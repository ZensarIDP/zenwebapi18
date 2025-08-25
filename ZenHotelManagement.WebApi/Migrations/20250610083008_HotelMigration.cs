using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZenHotelManagement.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class HotelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CabDrivers",
                columns: table => new
                {
                    CabDriverId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarVendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabDrivers", x => x.CabDriverId);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdharNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAc = table.Column<bool>(type: "bit", nullable: false),
                    CostPerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoomStatus = table.Column<bool>(type: "bit", nullable: false),
                    IsClean = table.Column<bool>(type: "bit", nullable: false),
                    RoomCapacity = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabBooking",
                columns: table => new
                {
                    CabBookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickUpDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DropOffDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickUpLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DropOffLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CabDriverId = table.Column<int>(type: "int", nullable: false),
                    Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsRideCompleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabBooking", x => x.CabBookingId);
                    table.ForeignKey(
                        name: "FK_CabBooking_CabDrivers_CabDriverId",
                        column: x => x.CabDriverId,
                        principalTable: "CabDrivers",
                        principalColumn: "CabDriverId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CabBooking_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PendingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CabDrivers",
                columns: new[] { "CabDriverId", "Age", "CarType", "CarVendor", "Gender", "IsAvailable", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 101, 23, "Sedan", "City Cabs", "Male", true, false, "Om Patil" },
                    { 102, 30, "Sedan", "Metro Taxi", "Male", true, false, "Rajesh Kumar" },
                    { 103, 28, "SUV", "Urban Rides", "Female", true, false, "Priya Sharma" },
                    { 104, 35, "Hatchback", "Express Cabs", "Male", true, false, "Amit Singh" },
                    { 105, 26, "Sedan", "Quick Taxi", "Female", true, false, "Sneha Patel" },
                    { 106, 40, "SUV", "Reliable Rides", "Male", true, false, "Vikram Rao" },
                    { 107, 28, "Hatchback", "Urban Rides", "Female", true, false, "Aarti Sharma" },
                    { 108, 35, "SUV", "Metro Taxis", "Male", true, false, "Ravi Kumar" },
                    { 109, 30, "Sedan", "Quick Cabs", "Female", true, false, "Sneha Desai" },
                    { 110, 27, "Sedan", "Urban Rides", "Female", true, false, "Priya Joshi" },
                    { 111, 32, "Hatchback", "Metro Taxis", "Male", true, false, "Amit Patel" },
                    { 112, 29, "SUV", "Quick Cabs", "Female", true, false, "Kavita Rao" },
                    { 113, 45, "Sedan", "City Cabs", "Male", true, false, "Rajesh Gupta" },
                    { 114, 33, "SUV", "Urban Rides", "Female", true, false, "Meera Nair" },
                    { 115, 38, "Sedan", "Metro Taxis", "Male", true, false, "Suresh Reddy" },
                    { 116, 29, "Hatchback", "Quick Cabs", "Female", true, false, "Anita Verma" },
                    { 117, 36, "SUV", "City Cabs", "Male", true, false, "Manoj Kumar" },
                    { 118, 27, "Sedan", "Urban Rides", "Female", true, false, "Pooja Singh" },
                    { 119, 31, "Hatchback", "Metro Taxis", "Male", true, false, "Karan Mehta" },
                    { 120, 34, "SUV", "Quick Cabs", "Female", true, false, "Divya Sharma" }
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Address", "Country", "CustomerId", "Gender", "IdType", "MobileNo", "Name" },
                values: new object[,]
                {
                    { 101, "Karvenagar, Pune", "India", "123412341234", "Female", "Aadhar Card", "+919765453674", "Samiksha Shelke" },
                    { 102, "Kothrud, Pune", "India", "432143214321", "Male", "Passport", "+919876543210", "Rohan Deshmukh" },
                    { 103, "Baner, Pune", "India", "567856785678", "Female", "Driving License", "+919876543211", "Priya Sharma" },
                    { 104, "Aundh, Pune", "India", "876587588765", "Male", "Aadhar Card", "+919876543212", "Amit Patil" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "AdharNo", "Age", "Email", "Gender", "IsDeleted", "MobileNo", "Name", "Position", "Salary" },
                values: new object[,]
                {
                    { 101, "123412341234", 23, "yash@gmail.com", "Male", false, "9834452728", "Yash Chutake", "Manager", 70000.00m },
                    { 102, "234523452345", 22, "rushikesh@gmail.com", "Male", false, "9876543210", "Rushikesh Kolte", "Accountant", 45000.00m },
                    { 103, "345634563456", 25, "shaibaz@gmail.com", "Male", false, "9123456789", "Shaibaz Hasware", "Kitchen Staff", 40000.00m },
                    { 104, "456745674567", 21, "abhishek@gmail.com", "Male", false, "9988776655", "Abhishek Khodade", "Front Desk Clerk", 40000.00m },
                    { 105, "567856785678", 27, "yashkashid@gmail.com", "Male", false, "9876543211", "Yash Kashid", "Kitchen Staff", 42000.00m },
                    { 106, "678967896789", 24, "samiksha@gmail.com", "Female", false, "9123456780", "Samiksha Shelke", "Accountant", 40000.00m },
                    { 107, "123412341234", 45, "tony@stark.com", "Male", false, "9123456781", "Tony Stark", "Room service", 12000.00m },
                    { 108, "234523452345", 43, "john@wwe.com", "Male", false, "9123456782", "John Cena", "Manager", 85000.00m },
                    { 109, "345634563456", 41, "randy@wwe.com", "Male", false, "9123456783", "Randy Orton", "Housekeeping", 60000.00m },
                    { 110, "456745674567", 44, "brock@wwe.com", "Male", false, "9123456784", "Brock Lesnar", "Waiter", 50000.00m },
                    { 111, "567856785678", 48, "reha@gmail.com", "Female", false, "9123456785", "Reha Repli", "Waitress", 50000.00m },
                    { 112, "678967896789", 40, "brucewayne@g.com", "Male", false, "9123456786", "Bruce Wayne", "Manager", 15000.00m },
                    { 113, "789078907890", 35, "clark.kent@gl.com", "Male", false, "9123456787", "Clark Kent", "Waiter", 55000.00m }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CostPerNight", "IsAc", "IsClean", "IsDeleted", "RoomCapacity", "RoomNo", "RoomStatus", "RoomType" },
                values: new object[,]
                {
                    { 1, 1000.00m, true, true, false, 2, "A-101", true, "Single Bed" },
                    { 2, 1500.00m, true, true, false, 2, "A-102", true, "Double Bed" },
                    { 3, 2000.00m, true, true, false, 2, "A-103", true, "Semi Deluxe" },
                    { 4, 2500.00m, true, false, false, 2, "A-104", false, "Deluxe" },
                    { 5, 800.00m, false, true, false, 2, "B-201", true, "Single Bed" },
                    { 6, 1200.00m, true, false, false, 3, "B-202", true, "Double Bed" },
                    { 7, 1800.00m, true, true, false, 4, "B-203", false, "Semi Deluxe" },
                    { 8, 3000.00m, true, true, false, 3, "B-204", true, "Deluxe" },
                    { 9, 900.00m, false, true, false, 1, "C-301", true, "Single Bed" },
                    { 10, 1600.00m, true, false, false, 2, "C-302", false, "Double Bed" },
                    { 11, 2200.00m, true, true, false, 3, "C-303", true, "Semi Deluxe" },
                    { 12, 2800.00m, true, false, false, 4, "C-304", true, "Deluxe" },
                    { 13, 750.00m, false, true, false, 1, "D-401", true, "Single Bed" },
                    { 14, 1400.00m, true, true, false, 2, "D-402", false, "Double Bed" },
                    { 15, 2100.00m, true, false, false, 3, "D-403", true, "Semi Deluxe" },
                    { 16, 3200.00m, true, true, false, 4, "D-404", true, "Deluxe" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_CabBooking_CabDriverId",
                table: "CabBooking",
                column: "CabDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_CabBooking_CustomerId",
                table: "CabBooking",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "CabBooking");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "CabDrivers");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
