using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TayarDelivery.Data.Migrations
{
    public partial class initdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(nullable: true),
                    LicensedNumber = table.Column<string>(nullable: true),
                    FilePathCompanyLogo = table.Column<string>(nullable: true),
                    WorkTime = table.Column<string>(nullable: true),
                    WhatsUpNumber = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    SupportNumber = table.Column<string>(nullable: true),
                    SupportEmail = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    facebookLink = table.Column<string>(nullable: true),
                    twitterLink = table.Column<string>(nullable: true),
                    instgramLink = table.Column<string>(nullable: true),
                    googlePlayAppLink = table.Column<string>(nullable: true),
                    appStoreAppLink = table.Column<string>(nullable: true),
                    CompanyLongitude = table.Column<double>(nullable: true),
                    CompanyLatitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainTitle = table.Column<string>(nullable: true),
                    MainTitleColor = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    SubTitleColor = table.Column<string>(nullable: true),
                    BackgroundImage = table.Column<string>(nullable: true),
                    BackgroundImageWidth = table.Column<string>(nullable: true),
                    BackgroundImageHeight = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegisterTrader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameSocial = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterTrader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreasPrice",
                columns: table => new
                {
                    ReceverAreaId = table.Column<int>(nullable: false),
                    DealerAreaId = table.Column<int>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Price = table.Column<float>(nullable: true),
                    DiscountPriceWhenReturn = table.Column<float>(nullable: true),
                    CanDiscount = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreasPrice", x => new { x.DealerAreaId, x.ReceverAreaId });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    MobileNumber1 = table.Column<string>(nullable: true),
                    MobileNumber2 = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    AreaId = table.Column<int>(nullable: true),
                    UserTypeID = table.Column<int>(nullable: true),
                    PriceTypeId = table.Column<int>(nullable: true),
                    DriverLongitude = table.Column<double>(nullable: true),
                    DriverLatitude = table.Column<double>(nullable: true),
                    DriverCarModel = table.Column<string>(nullable: true),
                    DriverCarType = table.Column<string>(nullable: true),
                    DriverCarNumber = table.Column<string>(nullable: true),
                    FcmToken = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
                    ConfirmMobileCode = table.Column<int>(nullable: true),
                    ForgetPasswordCode = table.Column<int>(nullable: true),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Area_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Area_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillTahsil",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    TraderId = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<float>(nullable: true),
                    NumberOfOrder = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillTahsil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillTahsil_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillTahsil_AspNetUsers_TraderId",
                        column: x => x.TraderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillTahsil_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Messege = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactUs_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactUs_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Link",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Decription = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    IsShow = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IconName = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Link", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Link_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Link_Link_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Link",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Link_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageSMS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    SendDateAt = table.Column<DateTime>(nullable: true),
                    ReceverId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageSMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageSMS_AspNetUsers_ReceverId",
                        column: x => x.ReceverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderContent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderContent_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderContent_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    TitlePrograming = table.Column<string>(nullable: true),
                    TitleView = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatus_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderStatus_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DiscountPercentage = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderType_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderType_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DiscountPercentage = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceType_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceType_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IconName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RatingCompany = table.Column<float>(nullable: true),
                    AllowNotification = table.Column<bool>(nullable: false),
                    AllowEdit = table.Column<bool>(nullable: false),
                    AllowAdd = table.Column<bool>(nullable: false),
                    AllowRemove = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserProfile_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    TitlePrograming = table.Column<string>(nullable: true),
                    TitleView = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserType_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserType_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<string>(nullable: true),
                    NameReceiver = table.Column<string>(nullable: true),
                    PhoneNumberReceiver = table.Column<string>(nullable: true),
                    PhoneNumberReceiver2 = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MainPrice = table.Column<float>(nullable: true),
                    TotalPrice = table.Column<float>(nullable: true),
                    AdditionalCost = table.Column<float>(nullable: true),
                    DiscountedCost = table.Column<float>(nullable: true),
                    AllowEdit = table.Column<bool>(nullable: false),
                    IsDeliveredSuccess = table.Column<bool>(nullable: false),
                    IsIncludeDeliveryPrice = table.Column<bool>(nullable: true),
                    DriverApprovalStatus = table.Column<bool>(nullable: false),
                    IsSetDriver = table.Column<bool>(nullable: false),
                    IsArchive = table.Column<bool>(nullable: false),
                    NoteTrader = table.Column<string>(nullable: true),
                    NoteDriver = table.Column<string>(nullable: true),
                    FilePathTraderSignature = table.Column<string>(nullable: true),
                    CustomerLongitude = table.Column<double>(nullable: true),
                    CustomerLatitude = table.Column<double>(nullable: true),
                    AreaIdSender = table.Column<int>(nullable: true),
                    AreaIdReceiver = table.Column<int>(nullable: true),
                    UserTraderId = table.Column<string>(nullable: true),
                    UserDriverId = table.Column<string>(nullable: true),
                    OrderStatusId = table.Column<int>(nullable: true),
                    OrderTypeId = table.Column<int>(nullable: true),
                    OrderContentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Area_AreaIdReceiver",
                        column: x => x.AreaIdReceiver,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Area_AreaIdSender",
                        column: x => x.AreaIdSender,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_OrderContent_OrderContentId",
                        column: x => x.OrderContentId,
                        principalTable: "OrderContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_OrderStatus_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_OrderType_OrderTypeId",
                        column: x => x.OrderTypeId,
                        principalTable: "OrderType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UserDriverId",
                        column: x => x.UserDriverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UserTraderId",
                        column: x => x.UserTraderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleLinks",
                columns: table => new
                {
                    LinkId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleLinks", x => new { x.RoleId, x.LinkId });
                    table.ForeignKey(
                        name: "FK_RoleLinks_Link_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Link",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleLinks_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolesUser",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesUser", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RolesUser_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolesUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesUser_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolesUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    SendDateAt = table.Column<DateTime>(nullable: true),
                    Status = table.Column<bool>(nullable: true),
                    OrderId = table.Column<int>(nullable: true),
                    ReceverId = table.Column<string>(nullable: true),
                    SenderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_ReceverId",
                        column: x => x.ReceverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    CreateByUserId = table.Column<string>(nullable: true),
                    UpdateByUserId = table.Column<string>(nullable: true),
                    UpdateAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHistory_AspNetUsers_CreateByUserId",
                        column: x => x.CreateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderHistory_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderHistory_AspNetUsers_UpdateByUserId",
                        column: x => x.UpdateByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Area_CreateByUserId",
                table: "Area",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Area_UpdateByUserId",
                table: "Area",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AreasPrice_CreateByUserId",
                table: "AreasPrice",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AreasPrice_ReceverAreaId",
                table: "AreasPrice",
                column: "ReceverAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AreasPrice_UpdateByUserId",
                table: "AreasPrice",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AreaId",
                table: "AspNetUsers",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PriceTypeId",
                table: "AspNetUsers",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserTypeID",
                table: "AspNetUsers",
                column: "UserTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_BillTahsil_CreateByUserId",
                table: "BillTahsil",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BillTahsil_TraderId",
                table: "BillTahsil",
                column: "TraderId");

            migrationBuilder.CreateIndex(
                name: "IX_BillTahsil_UpdateByUserId",
                table: "BillTahsil",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_CreateByUserId",
                table: "ContactUs",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_UpdateByUserId",
                table: "ContactUs",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Link_CreateByUserId",
                table: "Link",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Link_ParentId",
                table: "Link",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Link_UpdateByUserId",
                table: "Link",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageSMS_ReceverId",
                table: "MessageSMS",
                column: "ReceverId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_OrderId",
                table: "Notification",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ReceverId",
                table: "Notification",
                column: "ReceverId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_SenderId",
                table: "Notification",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AreaIdReceiver",
                table: "Order",
                column: "AreaIdReceiver");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AreaIdSender",
                table: "Order",
                column: "AreaIdSender");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreateByUserId",
                table: "Order",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderContentId",
                table: "Order",
                column: "OrderContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderStatusId",
                table: "Order",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderTypeId",
                table: "Order",
                column: "OrderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UpdateByUserId",
                table: "Order",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserDriverId",
                table: "Order",
                column: "UserDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserTraderId",
                table: "Order",
                column: "UserTraderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderContent_CreateByUserId",
                table: "OrderContent",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderContent_UpdateByUserId",
                table: "OrderContent",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_CreateByUserId",
                table: "OrderHistory",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_OrderId",
                table: "OrderHistory",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_UpdateByUserId",
                table: "OrderHistory",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_CreateByUserId",
                table: "OrderStatus",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_UpdateByUserId",
                table: "OrderStatus",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderType_CreateByUserId",
                table: "OrderType",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderType_UpdateByUserId",
                table: "OrderType",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceType_CreateByUserId",
                table: "PriceType",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceType_UpdateByUserId",
                table: "PriceType",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreateByUserId",
                table: "Role",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_UpdateByUserId",
                table: "Role",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleLinks_LinkId",
                table: "RoleLinks",
                column: "LinkId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesUser_CreateByUserId",
                table: "RolesUser",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesUser_UpdateByUserId",
                table: "RolesUser",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesUser_UserId",
                table: "RolesUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CreateByUserId",
                table: "Services",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_UpdateByUserId",
                table: "Services",
                column: "UpdateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserType_CreateByUserId",
                table: "UserType",
                column: "CreateByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserType_UpdateByUserId",
                table: "UserType",
                column: "UpdateByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AreasPrice_AspNetUsers_CreateByUserId",
                table: "AreasPrice",
                column: "CreateByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AreasPrice_AspNetUsers_UpdateByUserId",
                table: "AreasPrice",
                column: "UpdateByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AreasPrice_Area_DealerAreaId",
                table: "AreasPrice",
                column: "DealerAreaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AreasPrice_Area_ReceverAreaId",
                table: "AreasPrice",
                column: "ReceverAreaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Area_AreaId",
                table: "AspNetUsers",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PriceType_PriceTypeId",
                table: "AspNetUsers",
                column: "PriceTypeId",
                principalTable: "PriceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserType_UserTypeID",
                table: "AspNetUsers",
                column: "UserTypeID",
                principalTable: "UserType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_AspNetUsers_CreateByUserId",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_Area_AspNetUsers_UpdateByUserId",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceType_AspNetUsers_CreateByUserId",
                table: "PriceType");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceType_AspNetUsers_UpdateByUserId",
                table: "PriceType");

            migrationBuilder.DropForeignKey(
                name: "FK_UserType_AspNetUsers_CreateByUserId",
                table: "UserType");

            migrationBuilder.DropForeignKey(
                name: "FK_UserType_AspNetUsers_UpdateByUserId",
                table: "UserType");

            migrationBuilder.DropTable(
                name: "AreasPrice");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BillTahsil");

            migrationBuilder.DropTable(
                name: "CompanyInformation");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "HomeInfo");

            migrationBuilder.DropTable(
                name: "MessageSMS");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OrderHistory");

            migrationBuilder.DropTable(
                name: "RegisterTrader");

            migrationBuilder.DropTable(
                name: "RoleLinks");

            migrationBuilder.DropTable(
                name: "RolesUser");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Link");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "OrderContent");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "OrderType");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "PriceType");

            migrationBuilder.DropTable(
                name: "UserType");
        }
    }
}
