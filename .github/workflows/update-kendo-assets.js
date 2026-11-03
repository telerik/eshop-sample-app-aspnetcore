const { execFileSync } = require("child_process");
const fs = require("fs");
const path = require("path");

const [kendoVersion, themesVersion, ...options] = process.argv.slice(2);
const versionPattern = /^\d+\.\d+\.\d+$/;
const hashPattern = /^[A-Za-z0-9+/]{43}=$/;
const tagPattern = /<(?:script|link)\b[^>]*>/gi;
const assetPattern = /https:\/\/cdn\.jsdelivr\.net\/npm\/(\@{1,2}progress\/(?:kendo-ui|kendo-theme-[\w-]+))@(\d+\.\d+\.\d+)\/([^"'?#\s>]+)/i;
const dryRun = options.includes("--dry-run");
const rootOption = options.find((option) => option.startsWith("--root="));
const rootDirectory = path.resolve(rootOption ? rootOption.slice("--root=".length) : process.cwd());
const metadataRequests = new Map();

if (!versionPattern.test(kendoVersion || "") || !versionPattern.test(themesVersion || "")) {
    throw new Error("Usage: node update-kendo-assets.js <kendo-version> <themes-version> [--dry-run] [--root=directory]");
}

function sourceFiles() {
    return execFileSync("git", ["ls-files", "-z", "--", "*.html", "*.htm", "*.cshtml"], { cwd: rootDirectory })
        .toString("utf8").split("\0").filter(Boolean).map((file) => path.join(rootDirectory, file));
}

function packageMetadata(packageName, version) {
    const key = `${packageName}@${version}`;
    if (!metadataRequests.has(key)) {
        metadataRequests.set(key, (async () => {
            const url = `https://data.jsdelivr.com/v1/package/npm/${key}/flat`;
            try {
                const response = await fetch(url, { headers: { accept: "application/json" } });
                if (!response.ok) {
                    console.warn(`jsDelivr integrity metadata is unavailable for ${key}; the supplied version will be used and the integrity attribute will not be applied.`);
                    return null;
                }

                const metadata = await response.json();
                return new Map((metadata.files || []).map((file) => [file.name.replace(/^\//, ""), file.hash]));
            } catch (error) {
                console.warn(`jsDelivr integrity metadata is unavailable for ${key}; the supplied version will be used and the integrity attribute will not be applied.`);
                return null;
            }
        })());
    }

    return metadataRequests.get(key);
}

function updateTag(tag, asset, integrity) {
    let result = tag.replace(asset.url, asset.url.replace(`@${asset.currentVersion}/`, `@${asset.targetVersion}/`));
    const integrityPattern = /(\sintegrity\s*=\s*)(["'])[^"']*\2/i;
    const crossoriginPattern = /(\scrossorigin\s*=\s*)(["'])[^"']*\2/i;
    result = result.replace(integrityPattern, "").replace(crossoriginPattern, "");

    if (integrity) {
        result = result.replace(/\s*\/?>$/, (closing) => ` integrity="${integrity}" crossorigin="anonymous"${closing}`);
    }

    return result;
}

async function main() {
    const plans = [];
    let assetCount = 0;

    for (const filePath of sourceFiles()) {
        const content = fs.readFileSync(filePath, "utf8");
        const assets = [];

        for (const match of content.matchAll(tagPattern)) {
            const urlMatch = match[0].match(assetPattern);
            if (!urlMatch) {
                continue;
            }

            const packageName = urlMatch[1].replace(/^@@/, "@");
            const currentVersion = urlMatch[2];
            const targetVersion = packageName === "@progress/kendo-ui" ? kendoVersion : themesVersion;
            const filePath = urlMatch[3];
            assets.push({ tag: match[0], url: urlMatch[0], packageName, currentVersion, targetVersion, filePath });
        }

        if (assets.length) {
            plans.push({ filePath, content, assets });
            assetCount += assets.length;
        }
    }

    if (!assetCount) {
        throw new Error(`No pinned jsDelivr Kendo UI or theme tags were found under ${rootDirectory}`);
    }

    const keys = [...new Set(plans.flatMap((plan) => plan.assets.map((asset) => `${asset.packageName}@${asset.targetVersion}`)))];
    const metadataEntries = await Promise.all(keys.map(async (key) => {
        const separator = key.lastIndexOf("@");
        const packageName = key.slice(0, separator);
        const version = key.slice(separator + 1);
        return [key, await packageMetadata(packageName, version)];
    }));
    const metadata = new Map(metadataEntries);
    const updates = [];

    for (const plan of plans) {
        let content = plan.content;
        for (const asset of plan.assets) {
            const packageKey = `${asset.packageName}@${asset.targetVersion}`;
            const packageMetadata = metadata.get(packageKey);
            const hash = packageMetadata && packageMetadata.get(asset.filePath);
            if (typeof hash !== "string" || !hashPattern.test(hash)) {
                console.warn(`jsDelivr integrity metadata is unavailable for ${packageKey}/${asset.filePath}; the supplied version will be used and the integrity attribute will not be applied.`);
                content = content.replace(asset.tag, updateTag(asset.tag, asset, null));
                continue;
            }

            content = content.replace(asset.tag, updateTag(asset.tag, asset, `sha256-${hash}`));
        }
        updates.push({ filePath: plan.filePath, content, original: plan.content });
    }

    for (const update of updates) {
        if (update.content === update.original) {
            continue;
        }
        if (dryRun) {
            console.log(`Would update ${path.relative(rootDirectory, update.filePath)}`);
        } else {
            fs.writeFileSync(update.filePath, update.content);
        }
    }

    console.log(`${dryRun ? "Validated" : "Updated"} ${assetCount} Kendo asset tags in ${plans.length} files.`);
}

main().catch((error) => {
    console.error(error.message);
    process.exitCode = 1;
});