/// <binding AfterBuild='min' />
"use strict";

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    cleanCss = require("gulp-clean-css"),
    merge = require("merge-stream"),
    del = require("del"),
    bundleconfig = require("./bundleconfig.json");

var regex = {
    css: /\.css$/
};

function getBundles(regexPattern) {
    return bundleconfig.filter(function (bundle) {
        return regexPattern.test(bundle.outputFileName);
    });
}

function clean() {
    var files = bundleconfig.map(function (bundle) {
        return bundle.outputFileName;
    });
    return del(files);
}
gulp.task(clean);

function minCss() {
    var tasks = getBundles(regex.css).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(cleanCss())
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
}
gulp.task(minCss);

gulp.task("min", gulp.series(clean, minCss));