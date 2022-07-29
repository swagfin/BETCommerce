using Microsoft.EntityFrameworkCore.Migrations;

namespace BetCommerce.DataAccess.Migrations
{
    public partial class initDemoData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        SET IDENTITY_INSERT [dbo].[ProductCategories] ON
        INSERT INTO [dbo].[ProductCategories] ([Id], [CategoryName], [MoreInfo]) VALUES (1, N'T-SHIRTS', N'T-SHIRTS');
        INSERT INTO [dbo].[ProductCategories] ([Id], [CategoryName], [MoreInfo]) VALUES (2, N'JACKETS', N'JACKETS');
        INSERT INTO [dbo].[ProductCategories] ([Id], [CategoryName], [MoreInfo]) VALUES (3, N'TROUSERS', N'TROUSERS');
        INSERT INTO [dbo].[ProductCategories] ([Id], [CategoryName], [MoreInfo]) VALUES (4, N'WATCHES', N'WATCHES');
        INSERT INTO [dbo].[ProductCategories] ([Id], [CategoryName], [MoreInfo]) VALUES (5, N'SHOES', N'SHOES');
        INSERT INTO [dbo].[ProductCategories] ([Id], [CategoryName], [MoreInfo]) VALUES (6, N'OTHERS', N'OTHERS');
        SET IDENTITY_INSERT [dbo].[ProductCategories] OFF
        ");

            migrationBuilder.Sql(@"
        INSERT INTO [dbo].[Products] ([ProductBarcode], [ProductName], [Category], [Description], [CurrentQuantity], [SellingPrice], [ImageFile], [RegisteredDateUtc], [IsFeatured], [IsActive]) VALUES (N'54854', N'T-Shirt Guchi', N'T-SHIRTS', N'Some Good Guchi', 18, 450, N'tshirt.png', N'2022-07-29 05:51:11', 0, 1);
        INSERT INTO [dbo].[Products] ([ProductBarcode], [ProductName], [Category], [Description], [CurrentQuantity], [SellingPrice], [ImageFile], [RegisteredDateUtc], [IsFeatured], [IsActive]) VALUES (N'74562', N'Lady Guccie Top', N'T-SHIRTS', N'No details', 31, 400, N'ladygucci.png', N'2022-07-29 05:51:11', 0, 1);
        INSERT INTO [dbo].[Products] ([ProductBarcode], [ProductName], [Category], [Description], [CurrentQuantity], [SellingPrice], [ImageFile], [RegisteredDateUtc], [IsFeatured], [IsActive]) VALUES (N'25654', N'Jacket Gucci XL', N'JACKETS', N'No details', 12, 450, N'jacket.png', N'2022-07-29 05:51:11', 0, 1);
        INSERT INTO [dbo].[Products] ([ProductBarcode], [ProductName], [Category], [Description], [CurrentQuantity], [SellingPrice], [ImageFile], [RegisteredDateUtc], [IsFeatured], [IsActive]) VALUES (N'85005', N'Trouse 4 HF', N'TROUSERS', N'No details', 20, 600, N'trouser.png', N'2022-07-29 05:51:11', 0, 1);
        INSERT INTO [dbo].[Products] ([ProductBarcode], [ProductName], [Category], [Description], [CurrentQuantity], [SellingPrice], [ImageFile], [RegisteredDateUtc], [IsFeatured], [IsActive]) VALUES (N'52005', N'Royal Watch 4G', N'WATCHES', N'No details', 15, 350, N'watch.png', N'2022-07-29 05:51:11', 0, 1);
        INSERT INTO [dbo].[Products] ([ProductBarcode], [ProductName], [Category], [Description], [CurrentQuantity], [SellingPrice], [ImageFile], [RegisteredDateUtc], [IsFeatured], [IsActive]) VALUES (N'54006', N'Loofer Shoes S42', N'SHOES', N'No details', 17, 1400, N'lover.png', N'2022-07-29 05:51:11', 0, 1);
        INSERT INTO [dbo].[Products] ([ProductBarcode], [ProductName], [Category], [Description], [CurrentQuantity], [SellingPrice], [ImageFile], [RegisteredDateUtc], [IsFeatured], [IsActive]) VALUES (N'37500', N'Towel XR', N'OTHERS', N'No details', 11, 750, N'towel.png', N'2022-07-29 05:51:11', 0, 1);
        INSERT INTO [dbo].[Products] ([ProductBarcode], [ProductName], [Category], [Description], [CurrentQuantity], [SellingPrice], [ImageFile], [RegisteredDateUtc], [IsFeatured], [IsActive]) VALUES (N'74562', N'Sneakers White', N'OTHERS', N'No details', 9, 250, N'sneakers.png', N'2022-07-29 05:51:11', 0, 1);
        ");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
