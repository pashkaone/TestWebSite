﻿using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using FormAuth.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

public class RolesController : Controller
{
    private ApplicationRoleManager RoleManager
    {
        get
        {
            return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
        }
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Index()
    {
        return View(RoleManager.Roles);
    }

    public ActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(CreateRoleModel model)
    {
        if (ModelState.IsValid)
        {
            IdentityResult result = await RoleManager.CreateAsync(new ApplicationRole
            {
                Name = model.Name,
                Description = model.Description
            });
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Что-то пошло не так");
            }
        }
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Edit(string id)
    {
        ApplicationRole role = await RoleManager.FindByIdAsync(id);
        if (role != null)
        {
            return View(new EditRoleModel { Id = role.Id, Name = role.Name, Description = role.Description });
        }
        return RedirectToAction("Index");
    }
      [HttpPost]
      public async Task<ActionResult> Edit(EditRoleModel model)
      {
          if (ModelState.IsValid)
          {
              ApplicationRole role = await RoleManager.FindByIdAsync(model.Id);
              if (role != null)
              {
                  role.Description = model.Description;
                  role.Name = model.Name;
                  IdentityResult result = await RoleManager.UpdateAsync(role);
                  if (result.Succeeded)
                  {
                      return RedirectToAction("Index");
                  }
                  else
                  {
                      ModelState.AddModelError("", "Что-то пошло не так");
                  }
              }
          }
          return View(model);
      } 

    public async Task<ActionResult> Delete(string id)
    {
        ApplicationRole role = await RoleManager.FindByIdAsync(id);
        if (role != null)
        {
            IdentityResult result = await RoleManager.DeleteAsync(role);
        }
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> EditRoles()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> EditRoles(string roleId, ApplicationContext context)
    {
  
        return RedirectToAction("EditRoles");
    }
}

   