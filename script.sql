-- Lệnh tạo Database
CREATE DATABASE warehouse_management
GO
USE warehouse_management
GO

-- Tạo bảng Category
CREATE TABLE Category (
    category_id   INT PRIMARY KEY IDENTITY(1,1),
    category_name NVARCHAR(100) NOT NULL UNIQUE       
);
GO

-- Tạo bảng Product
CREATE TABLE Product (
    product_id  INT PRIMARY KEY IDENTITY(1,1),
    product_name NVARCHAR(150) NOT NULL,
    description NVARCHAR(255),
    category_id  INT FOREIGN KEY REFERENCES Category(category_id)
);
GO

-- Tạo bảng Sku
CREATE TABLE Sku (
    sku_id INT PRIMARY KEY IDENTITY(1,1),
    product_id INT NOT NULL FOREIGN KEY REFERENCES Product(product_id),
    sku_code NVARCHAR(50) NOT NULL UNIQUE,            
    unit NVARCHAR(30)
);
GO

-- Tạo bảng Warehouse
CREATE TABLE Warehouse (
    warehouse_id INT PRIMARY KEY IDENTITY(1,1),
    warehouse_name NVARCHAR(150) NOT NULL UNIQUE,      
    address  NVARCHAR(250),
    max_capacity  INT NOT NUll 
);
GO

-- Tạo bảng StorageLocation
CREATE TABLE StorageLocation (
    location_id INT PRIMARY KEY IDENTITY(1,1),
    warehouse_id INT NOT NULL FOREIGN KEY REFERENCES Warehouse(warehouse_id),
    location_description NVARCHAR(150),
    capacity INT NOT NULL
);
GO

-- Tạo bảng Supplier
CREATE TABLE Supplier (
    supplier_id INT PRIMARY KEY IDENTITY(1,1),
    supplier_name NVARCHAR(150) NOT NULL,
    address NVARCHAR(250),
    email  NVARCHAR(100) UNIQUE,             
    phone  NVARCHAR(20)  UNIQUE                 
);
GO

-- Tạo bảng ImportReceipt
CREATE TABLE ImportReceipt (
    import_id  INT PRIMARY KEY IDENTITY(1,1),
    import_date DATE NOT NULL DEFAULT GETDATE()
);
GO

-- Tạo bảng ImportDetail
CREATE TABLE ImportDetail (
    import_detail_id INT PRIMARY KEY IDENTITY(1,1),
    import_id INT NOT NULL FOREIGN KEY REFERENCES ImportReceipt(import_id),
    sku_id INT NOT NULL FOREIGN KEY REFERENCES Sku(sku_id),
    supplier_id INT NOT NULL FOREIGN KEY REFERENCES Supplier(supplier_id),
    location_id INT NOT NULL FOREIGN KEY REFERENCES StorageLocation(location_id),
    quantity INT NOT NULL
);
GO

-- Tạo bảng ExportReceipt
CREATE TABLE ExportReceipt (
    export_id INT PRIMARY KEY IDENTITY(1,1),
    export_date DATE NOT NULL DEFAULT GETDATE(),
    purpose  NVARCHAR(200)
);
GO

-- Tạo bảng ExportDetail
CREATE TABLE ExportDetail (
    export_detail_id INT PRIMARY KEY IDENTITY(1,1),
    export_id INT NOT NULL FOREIGN KEY REFERENCES ExportReceipt(export_id),
    sku_id INT NOT NULL FOREIGN KEY REFERENCES Sku(sku_id),
    location_id INT NOT NULL FOREIGN KEY REFERENCES StorageLocation(location_id),
    quantity INT NOT NULL                     
);
GO

-- Tạo bảng Inventory
CREATE TABLE Inventory (
    location_id INT NOT NULL FOREIGN KEY REFERENCES StorageLocation(location_id),
    sku_id INT NOT NULL FOREIGN KEY REFERENCES Sku(sku_id),
    quantity INT NOT NULL DEFAULT 0,
    PRIMARY KEY (location_id, sku_id)
);
GO

-- Tạo bảng Account
CREATE TABLE Account (
    account_name NVARCHAR(20) PRIMARY KEY,
    display_name NVARCHAR(20) NOT NULL DEFAULT N'Quản lý kho',
    password NVARCHAR(1000) NOT NULL
);
GO

-- 2. RÀNG BUỘC (CONSTRAINTS)
ALTER TABLE Warehouse 
ADD CONSTRAINT Check_Warehouse_Capacity CHECK (max_capacity > 0);
GO

ALTER TABLE StorageLocation 
ADD CONSTRAINT Check_Location_Capacity CHECK (capacity > 0);
GO

ALTER TABLE Inventory 
ADD CONSTRAINT Check_Inventory_Quantity CHECK (quantity >= 0);
GO

ALTER TABLE ImportDetail 
ADD CONSTRAINT Check_Import_Quantity CHECK (quantity > 0);
GO

ALTER TABLE ExportDetail 
ADD CONSTRAINT Check_Export_Quantity CHECK (quantity > 0);
GO
-- 3. HÀM (FUNCTIONS) - Phải tạo trước View và Procedure
-- Tính tổng lưu trữ tại vị trí
CREATE FUNCTION UF_TongLuuTru (@LocationID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Tong INT
    SELECT @Tong= SUM(quantity) 
    FROM Inventory
    WHERE location_id=@LocationID
    RETURN @Tong
END
GO

-- Tính tổng một kho hàng
CREATE FUNCTION UF_TongKho (@WarehouseID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Tong INT
    SELECT @Tong=SUM(i.quantity) 
    FROM Inventory as i
    JOIN StorageLocation as sl ON i.location_id=sl.location_id
    WHERE sl.warehouse_id=@WarehouseID
    RETURN @Tong
END
GO

-- Tính số lượng theo sản phẩm
CREATE FUNCTION UF_TongSanPham (@SkuID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Tong INT
    SELECT @Tong=SUM(quantity)
    FROM Inventory
    WHERE sku_id=@SkuID
    RETURN @Tong
END
GO
-- 4. KHUNG NHÌN (VIEWS) - Phải tạo trước Procedure
-- Khung nhìn tồn kho hiển thị theo từng vị trí
CREATE VIEW UV_InventoryDetail
AS
    SELECT sl.location_id, sl.location_description, c.category_name, p.product_name, s.sku_code, s.unit, i.quantity
    FROM Inventory as i
    JOIN StorageLocation as sl on sl.location_id=i.location_id
    JOIN Sku as s on s.sku_id=i.sku_id
    JOIN Product as p on s.product_id=p.product_id
    LEFT JOIN Category as c on p.category_id=c.category_id
GO

-- Khung nhìn hiển thị tồn kho theo kho lưu trữ
CREATE VIEW UV_HienThiTongKho
AS
    SELECT  warehouse_id as N'Mã kho', warehouse_name as N'Tên kho', dbo.UF_TongKho(warehouse_id) as N'Số lượng'
    FROM Warehouse 
GO

-- Khung nhìn hiển thị tồn kho theo vị trí
CREATE VIEW UV_HienThiLuuTru
AS
    SELECT  location_id as N'Mã vị trí', location_description as N'Vị trí lưu trữ', dbo.UF_TongLuuTru(location_id) as N'Số lượng'
    FROM StorageLocation
GO

-- Khung nhìn hiển thị số lượng theo biến thể sản phẩm
CREATE VIEW UV_HienThiTongSanPham
AS
    SELECT  s.sku_code as N'Sku Code', dbo.UF_TongSanPham(s.sku_id) as N'Số lượng'
    FROM Sku as s
GO
-- 5. THỦ TỤC (PROCEDURES)
CREATE PROCEDURE USP_Login 
@UserName NVARCHAR(100), @Password NVARCHAR(20)
AS
BEGIN
    SELECT * FROM Account WHERE account_name=@UserName and password=@Password
END
GO

CREATE PROCEDURE USP_GetStorageLocation
AS
BEGIN
    SELECT * FROM StorageLocation
END
GO

CREATE PROCEDURE USP_GetInventoryByLocationID
@LocationID INT
AS 
BEGIN
    SELECT * FROM UV_InventoryDetail
    WHERE location_id=@LocationID
END
GO

CREATE PROCEDURE USP_InsertCategory @CategoryName NVARCHAR(100)
AS
BEGIN
    INSERT INTO Category(category_name) VALUES (@CategoryName)
END
GO

CREATE PROCEDURE USP_DeleteCategory @CategoryID INT
AS
BEGIN
    BEGIN Tran XoaDanhMuc
    BEGIN TRY
        UPDATE Product SET category_id = NULL WHERE category_id = @CategoryID
        DELETE FROM Category WHERE category_id = @CategoryID
        COMMIT TRAN XoaDanhMuc
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN XoaDanhMuc
    END CATCH
END
GO

CREATE PROCEDURE USP_UpdateCategory @CategoryID INT, @CategoryName NVARCHAR(100)
AS
BEGIN
    UPDATE Category SET category_name=ISNULL(@CategoryName,category_name) 
    WHERE category_id=@CategoryID
END
GO

CREATE PROCEDURE USP_ListCategory
AS
BEGIN
    SELECT * FROM Category
END
GO

CREATE PROCEDURE USP_GetCategoryByID @CategoryID INT
AS
BEGIN
    SELECT * FROM Category WHERE category_id=@CategoryID
END
GO

CREATE PROCEDURE USP_SearchCategoryByName @CategoryName NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Category WHERE category_name like '%'+@CategoryName+'%'
END
GO

CREATE PROCEDURE USP_InsertProduct
@ProductName NVARCHAR(100),@Description NVARCHAR(100),@CategoryID INT
AS
BEGIN
    INSERT INTO Product(product_name,description,category_id) VALUES (@ProductName,@Description,@CategoryID)
END
GO

CREATE PROCEDURE USP_DeleteProduct @ProductID INT
AS
BEGIN
    DELETE FROM PRODUCT WHERE product_id=@ProductID
END
GO

CREATE PROCEDURE USP_UpdateProduct
@ProductID INT,@ProductName NVARCHAR(100),@Description NVARCHAR(100),@CategoryID INT
AS
BEGIN
    UPDATE PRODUCT 
    SET product_name=ISNULL(@ProductName,product_name), description=ISNULL(@Description,description), category_id=ISNULL(@CategoryID,category_id)
    WHERE product_id=@ProductID
END
GO

CREATE PROCEDURE USP_ListProduct
AS
BEGIN
    SELECT * FROM Product
END
GO

CREATE PROCEDURE USP_GetProductByID
@ProductID INT
AS
BEGIN
    SELECT * FROM Product
    WHERE product_id=@ProductID
END
GO

CREATE PROCEDURE USP_SearchProductByName
@ProductName NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Product
    WHERE product_name like '%'+@ProductName+'%'
END
GO

CREATE PROCEDURE USP_InsertSku
@SkuCode NVARCHAR(100), @Unit NVARCHAR(50), @ProductID INT
AS
BEGIN
    INSERT INTO Sku(sku_code,unit,product_id) VALUES (@SkuCode,@Unit,@ProductID)
END
GO

CREATE PROCEDURE USP_DeleteSku
@SkuID INT
AS
BEGIN
    DELETE FROM Sku WHERE sku_id=@SkuID 
END
GO

CREATE PROCEDURE USP_UpdateSku
@SkuID INT,@SkuCode NVARCHAR(100), @Unit NVARCHAR(50), @ProductID INT
AS
BEGIN
    UPDATE Sku 
    SET  sku_code=ISNULL(@SkuCode,sku_code), unit=ISNULL(@Unit,unit),product_id=ISNULL(@ProductID,product_id)
    WHERE sku_id=@SkuID
END
GO

CREATE PROCEDURE USP_ListSku
AS
BEGIN
    SELECT * FROM Sku
END
GO

CREATE PROCEDURE USP_GetSkuByID
@SkuID INT
AS
BEGIN
    SELECT * FROM Sku
    WHERE sku_id=@SkuID
END
GO

CREATE PROCEDURE USP_SearchSkuByName
@SkuCode NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Sku
    WHERE sku_code LIKE '%'+@SkuCode+'%'
END
GO

CREATE PROCEDURE USP_InsertWarehouse
@WarehouseName NVARCHAR(100),@Address NVARCHAR(255), @MaxCapacity INT
AS
BEGIN
    INSERT INTO Warehouse(warehouse_name,address,max_capacity) 
    VALUES(@WarehouseName,@Address,@MaxCapacity)
END
GO

CREATE PROCEDURE USP_DeleteWarehouse
@WarehouseID INT
AS
BEGIN
    DELETE FROM Warehouse
    WHERE warehouse_id=@WarehouseID
END
GO

CREATE PROCEDURE USP_UpdateWarehouse
@WarehouseID INT,@WarehouseName NVARCHAR(100),@Address NVARCHAR(255), @MaxCapacity INT
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        UPDATE Warehouse 
        SET  warehouse_name=ISNULL(@WarehouseName,warehouse_name), address=ISNULL(@Address,address),max_capacity=ISNULL(@MaxCapacity,max_capacity)
        WHERE warehouse_id=@WarehouseID

        DECLARE @Tong INT,@SucChuaToiDa INT
        SELECT @SucChuaToiDa=max_capacity FROM Warehouse WHERE warehouse_id=@WarehouseID
        SELECT @Tong=ISNULL(SUM(capacity),0) FROM StorageLocation WHERE warehouse_id=@WarehouseID
        
        IF( @SucChuaToiDa >= @Tong )
        BEGIN
            PRINT N'Cập nhật kho hàng thành công'
            COMMIT TRANSACTION
        END
        ELSE
        BEGIN
            PRINT N'Vượt quá sức chứa kho hàng'
            ROLLBACK TRANSACTION
        END
    END TRY
    BEGIN CATCH
        PRINT N'Cập nhật kho hàng không thành công'
        ROLLBACK TRANSACTION
    END CATCH
END
GO

CREATE PROCEDURE USP_ListWarehouse
AS
BEGIN
    SELECT * FROM Warehouse
END
GO

CREATE PROCEDURE USP_GetWarehouseByID
@WarehouseID INT
AS
BEGIN
    SELECT * FROM Warehouse WHERE warehouse_id=@WarehouseID
END
GO

CREATE PROCEDURE USP_SearchWarehouseByName
@WarehouseName NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Warehouse 
    WHERE warehouse_name LIKE '%'+@WarehouseName+'%'
END
GO

CREATE PROCEDURE USP_InsertStorageLocation
@Description NVARCHAR(150),@Capacity INT, @WarehouseID INT
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        INSERT StorageLocation (location_description,capacity,warehouse_id)
        VALUES(@Description,@Capacity,@WarehouseID)
        
        DECLARE @Tong INT,@SucChuaToiDa INT
        SELECT @SucChuaToiDa=max_capacity FROM Warehouse WHERE warehouse_id=@WarehouseID
        SELECT @Tong=ISNULL(SUM(capacity),0) FROM StorageLocation WHERE warehouse_id=@WarehouseID
        
        IF( @SucChuaToiDa >= @Tong )
        BEGIN
            PRINT N'Thêm vị trí lưu trữ thành công'
            COMMIT TRANSACTION
        END
        ELSE
        BEGIN
            PRINT N'Vượt quá sức chứa kho hàng'
            ROLLBACK TRANSACTION
        END
    END TRY
    BEGIN CATCH
        PRINT N'Thêm vị trí lưu trữ thất bại'
        ROLLBACK TRANSACTION
    END CATCH
END
GO

CREATE PROCEDURE USP_DeleteStorageLocation
@LocationID INT
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        DELETE FROM StorageLocation WHERE location_id=@LocationID
        PRINT N'Xóa vị trí lưu trữ thành công'
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        PRINT N'Xóa vị trí lưu trữ không thành công'
        ROLLBACK TRANSACTION
    END CATCH
END
GO

CREATE PROCEDURE USP_UpdateStorageLocation
@LocationID INT,@Description NVARCHAR(150),@Capacity INT, @WarehouseID INT
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        UPDATE StorageLocation 
        SET location_description=ISNULL(@Description,location_description),
            capacity=ISNULL(@Capacity,capacity),
            warehouse_id=ISNULL(@WarehouseID,warehouse_id)
        WHERE location_id=@LocationID
        
        DECLARE @Tong INT,@SucChuaToiDa INT,@Quantity INT, @SucChua INT
        
        SELECT @SucChuaToiDa=max_capacity FROM Warehouse 
        WHERE warehouse_id IN (SELECT warehouse_id FROM StorageLocation WHERE location_id=@LocationID)
        
        SELECT @Tong=ISNULL(SUM(capacity),0)
        FROM StorageLocation 
        WHERE warehouse_id IN (SELECT warehouse_id FROM StorageLocation WHERE location_id=@LocationID)
        
        SET @Quantity=dbo.UF_TongLuuTru(@LocationID)
        SELECT @SucChua=capacity FROM StorageLocation WHERE location_id=@LocationID
        
        IF( @SucChuaToiDa >= @Tong and @SucChua >= @Quantity)
        BEGIN
            PRINT N'Cập nhật vị trí lưu trữ thành công'
            COMMIT TRANSACTION
        END
        ELSE
        BEGIN
            PRINT N'Sức chứa không hợp lệ'
            ROLLBACK TRANSACTION
        END
    END TRY
    BEGIN CATCH
        PRINT N'Cập nhật vị trí lưu trữ không thành công'
        ROLLBACK TRANSACTION
    END CATCH
END
GO

CREATE PROCEDURE USP_SearchLocationByDescription
@Description NVARCHAR(100)
AS
BEGIN
    SELECT * FROM StorageLocation WHERE location_description LIKE '%'+@Description+'%'
END
GO

CREATE PROCEDURE USP_InsertSupplier
@SupplierName NVARCHAR(100), @Address NVARCHAR(100),@Email NVARCHAR(255),@Phone NVARCHAR(50)
AS
BEGIN
    INSERT INTO Supplier(supplier_name, address,email,phone)
    VALUES(@SupplierName,@Address,@Email,@Phone)
END
GO

CREATE PROCEDURE USP_DeleteSupplier
@SupplierID INT
AS
BEGIN
    DELETE FROM Supplier WHERE supplier_id=@SupplierID
END
GO

CREATE PROCEDURE USP_ListSupplier
AS
BEGIN
    SELECT * FROM Supplier 
END
GO

CREATE PROCEDURE USP_GetSupplierByID
@SupplierID INT
AS
BEGIN
    SELECT * FROM Supplier WHERE supplier_id=@SupplierID
END
GO

CREATE PROCEDURE USP_SearchSupplierByName
@SupplierName NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Supplier WHERE supplier_name LIKE '%'+@SupplierName+'%'
END
GO

CREATE PROCEDURE USP_InsertExportReceipt
@Purpose NVARCHAR(155)
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        INSERT INTO ExportReceipt (purpose)
        VALUES (ISNULL(@Purpose,N'Xuất kho'))
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH
END
GO

CREATE PROCEDURE USP_InsertExportDetail
@ExportID INT, @SkuID INT, @LocationID INT, @Quantity INT
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        INSERT INTO ExportDetail(export_id,sku_id,location_id,quantity) 
        VALUES (@ExportID,@SkuID,@LocationID,@Quantity)

        UPDATE Inventory 
        SET quantity=quantity-@Quantity 
        WHERE location_id=@LocationID and sku_id=@SkuID
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH
END
GO

CREATE PROCEDURE USP_GetExportReceiptByDate
@NgayBatDau Date, @NgayKetThuc Date
AS
BEGIN
    SELECT * FROM ExportReceipt 
    WHERE export_date BETWEEN @NgayBatDau AND @NgayKetThuc
END
GO

CREATE PROCEDURE USP_InsertImportReceipt
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        INSERT INTO ImportReceipt DEFAULT VALUES
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH
END
GO

CREATE PROCEDURE USP_InsertImportDetail
@ImportID INT, @SkuID INT, @SupplierID INT, @LocationID INT, @Quantity INT
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        INSERT INTO ImportDetail(import_id,sku_id,supplier_id,location_id,quantity) 
        VALUES (@ImportID,@SkuID,@SupplierID,@LocationID,@Quantity)

        UPDATE Inventory 
        SET quantity=quantity+@Quantity 
        WHERE location_id=@LocationID and sku_id=@SkuID
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH
END
GO

CREATE PROCEDURE USP_GetImportReceiptByDate
@NgayBatDau Date, @NgayKetThuc Date
AS
BEGIN
    SELECT * FROM ImportReceipt 
    WHERE import_date BETWEEN @NgayBatDau AND @NgayKetThuc
END
GO
--- Tạo lỗi mất dữ liệu cập nhật ---
-- Thủ tục tạo chi tiết nhập kho và cập nhật tồn kho
CREATE PROCEDURE USP_NhapKho
@ImportID INT, @SkuID INT, @SupplierID INT, @LocationID INT, @Quantity INT 
AS 
BEGIN
    BEGIN TRANSACTION
        INSERT INTO ImportDetail (import_id,sku_id,supplier_id,location_id, quantity) 
        VALUES (@ImportID,@SkuID,@SupplierID,@LocationID,@Quantity)
    IF(@@ERROR <> 0)
    BEGIN 
        ROLLBACK TRAN 
    RETURN 
    END
    DECLARE @SoDu INT SELECT @SoDu=quantity FROM Inventory 
    WHERE location_id=@LocationID and sku_id=@SkuID
    WAITFOR DELAY '00:00:10'
    Update Inventory
    SET quantity= @SoDu+@Quantity WHERE location_id=@LocationID and sku_id=@SkuID
    IF(@@ERROR <> 0)
    BEGIN 
        ROLLBACK TRAN 
        RETURN 
    END 
        COMMIT TRANSACTION
END
GO
-- Thủ tục tạo chi tiết xuất kho và cập nhật tồn kho
CREATE PROCEDURE USP_XuatKho @ImportID INT,@SkuID INT,@LocationID INT, @Quantity INT 
AS
BEGIN
    BEGIN TRANSACTION
        INSERT INTO ExportDetail(export_id,sku_id,location_id,quantity) 
        VALUES (@ImportID,@SkuID,@LocationID,@Quantity)
    IF(@@ERROR <> 0)
    BEGIN 
        ROLLBACK TRAN 
        RETURN 
    END
    DECLARE @SoDu INT SELECT @SoDu=quantity 
    FROM Inventory WITH (NOLOCK)
    WHERE location_id=@LocationID AND sku_id=@SkuID
    WAITFOR DELAY '00:00:15'
    IF(@Quantity > @SoDu)
    BEGIN ROLLBACK TRAN RETURN END
    Update Inventory SET quantity=@SoDu-@Quantity 
    WHERE location_id=@LocationID 
    AND sku_id=@SkuID
    IF(@@ERROR <> 0)
    BEGIN 
    ROLLBACK TRAN 
    RETURN 
    END
    COMMIT TRANSACTION
END
GO
-- Giải pháp mất dữ liệu cập nhật
-- Lỗi Non-repeatable Read --
-- Tìm kiếm nhà cung cấp theo tên và số điện thoại
CREATE PROCEDURE USP_SearchSupplier 
@SupplierName NVARCHAR(100), @Phone NVARCHAR(50)
AS
BEGIN
    BEGIN TRANSACTION
    IF NOT EXISTS (SELECT * FROM Supplier WHERE supplier_name=@SupplierName AND phone=@Phone)
    BEGIN
        PRINT('Không tìm thấy')
        ROLLBACK TRAN
        RETURN
    END
    WAITFOR DELAY '00:00:10'
    SELECT * FROM Supplier WHERE supplier_name=@SupplierName AND phone=@Phone
    COMMIT TRAN
END
GO
-- Cập nhật thông tin nhà cung cấp
CREATE PROCEDURE USP_UpdateSupplier
@SupplierID INT,@SupplierName NVARCHAR(100), 
@Address NVARCHAR(100),@Email NVARCHAR(255),@Phone NVARCHAR(50)
AS
BEGIN
    BEGIN TRAN
    UPDATE Supplier SET 
        supplier_name=ISNULL(@SupplierName,supplier_name),
        address=ISNULL(@Address,address),
        email=ISNULL(@Email,email),
        phone=ISNULL(@Phone,phone)
    WHERE supplier_id=@SupplierID
    
    IF(@@ERROR <> 0)
    BEGIN
        ROLLBACK TRAN
        RETURN
    END
    COMMIT TRANSACTION
END
GO
-- Giải pháp lỗi không đọc lại được dữ liệu --


