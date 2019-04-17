﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Music.App.Modals;
using Music.DataAccess.Repositories;
using Music.Modals;

namespace Music.Controllers
{
    public class LibraryController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TagRepository _tagRepository;

        public LibraryController(
            UserManager<ApplicationUser> userManager,
            TagRepository tagRepository)
        {
            _userManager = userManager;
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllTags()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var tags = _tagRepository.All(userId);

            return Json(new
            {
                tags
            });
        }

        [HttpGet]
        public JsonResult GetColors()
        {
            var colors = _tagRepository.AllColors();
            return Json(new { colors });
        }

        [HttpPost]
        public JsonResult AddTagReference([FromBody]TagDetails tagDetails)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (tagDetails.TagId == null)
            {
                tagDetails.TagId = _tagRepository.Upload(userId, tagDetails.Tag, 2, tagDetails.Color);
            }
            _tagRepository.UploadReference(userId, (int)tagDetails.TagId, tagDetails.YouTubeId);
            return Json("");
        }

        [HttpPost]
        public JsonResult DeleteTagReference([FromBody]int referenceId)
        {
            _tagRepository.DeleteTagReference(referenceId);
            return Json("");
        }

        //[HttpPost]
        //public JsonResult AddTag([FromBody] string tag)
        //{
        //    _tagRepository.Upload(tag, 2);

        //    return Json("");
        //}
    }
}