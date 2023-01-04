using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using Telerik.Web.Captcha;
using Models.InputModels;

namespace Web.Controllers
{
    public class ContactsController : Controller
    {
        protected readonly IWebHostEnvironment HostingEnvironment;
        protected readonly string CaptchaPath;

        public ContactsController(IWebHostEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
            CaptchaPath = Path.Combine(hostingEnvironment.WebRootPath, "Contacts", "Captcha");

            if (!Directory.Exists(CaptchaPath))
            {
                Directory.CreateDirectory(CaptchaPath);
            }
        }

        public ActionResult Index()
        {
            GenerateNewCaptcha();

            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactsInputModel user, CaptchaInputModel captchaModel)
        {
            if (string.IsNullOrEmpty(captchaModel.CaptchaID))
            {
                GenerateNewCaptcha();

                return View();
            }
            else
            {
                string text = GetCaptchaText(captchaModel.CaptchaID);

                if (text == captchaModel.Captcha.ToUpperInvariant())
                {
                    ModelState.Clear();
                    GenerateNewCaptcha();
                }
            }

            return View();
        }

        private void GenerateNewCaptcha()
        {
            CaptchaImage captchaImage = SetCaptchaImage();

            ViewData["Captcha"] = Url.Content("~/Contacts/Captcha/" + captchaImage.UniqueId + ".png");
            ViewData["CaptchaID"] = captchaImage.UniqueId;
        }

        public ActionResult Reset_Events()
        {
            CaptchaImage newCaptcha = SetCaptchaImage();

            return Json(new CaptchaInputModel
            {
                Captcha = Url.Content("~/Contacts/Captcha/" + newCaptcha.UniqueId + ".png"),
                CaptchaID = newCaptcha.UniqueId
            });
        }

        public ActionResult Validate_Events(CaptchaInputModel model)
        {
            string text = GetCaptchaText(model.CaptchaID);

            return Json(text == model.Captcha.ToUpperInvariant());
        }


        private string GetCaptchaText(string captchaId)
        {
            string text = HttpContext.Session.GetString("captcha_" + captchaId);

            return text;
        }

        private CaptchaImage SetCaptchaImage()
        {
            CaptchaImage newCaptcha = CaptchaHelper.GetNewCaptcha();

            var image = CaptchaHelper.RenderCaptcha(newCaptcha);
            image.Save(Path.Combine(CaptchaPath, newCaptcha.UniqueId + ".png"), ImageFormat.Png);

            HttpContext.Session.SetString("captcha_" + newCaptcha.UniqueId, newCaptcha.Text);

            return newCaptcha;
        }
    }
}
