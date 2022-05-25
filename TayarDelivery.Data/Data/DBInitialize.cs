using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayarDelivery.Entity.Domins;

namespace TayarDelivery.Data.Data
{
    public class DBInitialize
    {
        public static async void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                var _userManager =
                         serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var _roleManager =
                         serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                //-------------- Initialize System Roles -------------------
                try
                {
                    if (!context.Roles.Any())
                    {
                        IdentityResult result = await _roleManager.CreateAsync(new IdentityRole("administrator"));
                        IdentityResult result2 = await _roleManager.CreateAsync(new IdentityRole("manager"));
                        IdentityResult result3 = await _roleManager.CreateAsync(new IdentityRole("trader"));
                        IdentityResult result4 = await _roleManager.CreateAsync(new IdentityRole("driver"));
                        IdentityResult result5 = await _roleManager.CreateAsync(new IdentityRole("accountant"));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                //-------------- Initialize System Roles -------------------
                try
                {
                    if (!context.UserType.Any())
                    {
                        IDictionary<string, string> arrayUserTypes
                            = new Dictionary<string, string> {
                                { "administrator", "مسؤول" },
                                { "manager", "مدير" },
                                { "trader", "تاجر" },
                                { "driver", "سائق" },
                                { "accountant", "محاسب" },
                            };
                        foreach (var item in arrayUserTypes)
                        {
                            var entity = new UserType {
                                TitlePrograming = item.Key, 
                                TitleView = item.Value,
                                CreateAt = DateTime.Now
                            };
                            context.UserType.Add(entity);
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                //-------------- Initialize Order Status -------------------
                try
                {
                    if (!context.OrderStatus.Any())
                    {
                        IDictionary<string, string> arrayUserTypes
                            = new Dictionary<string, string> {
                                { "waiting", "قيد الإنتظار" },
                                { "receivedcompany", "تم الاستلام بالشركة" },
                                { "beingdelivered", "قيد التوصيل" },
                                { "donedelivered", "تم التوصيل" },
                                { "canceled", "ملغي" },
                                { "rejected", "مرفوض" },
                            };
                        foreach (var item in arrayUserTypes)
                        {
                            var entity = new OrderStatus
                            {
                                TitlePrograming = item.Key,
                                TitleView = item.Value,
                                CreateAt = DateTime.Now
                            };
                            context.OrderStatus.Add(entity);
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                //-------------- Initialize System Admin -------------------
                try
                {
                    if (!context.Users.Any())
                    {
                        var EmailAdmin = "admin@gmail.com";
                        var PasswordAdmin = "Admin11$";
                        var PhoneNumberAdmin = "10002000";
                        var FullName = "Admin Admin";
                        var user = new ApplicationUser
                        {
                            UserName = EmailAdmin,
                            Email = EmailAdmin,
                            PhoneNumber = PhoneNumberAdmin,
                            FullName = FullName,
                            EmailConfirmed = true,
                            IsActive = true,
                            CreateAt = DateTime.Now,
                            UserTypeID = context.UserType.SingleOrDefault(c => c.TitlePrograming.Equals("administrator")).Id
                        };

                        var result = await _userManager.CreateAsync(user, PasswordAdmin);
                        var result2 = await _userManager.AddToRoleAsync(user, "administrator");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
