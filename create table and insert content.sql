create table OrderDetails(
	id int primary key identity(1, 1),
	price decimal(10, 2) not null,
	description nvarchar(256) not null,
	hasdiscount bit not null
)

go


insert into OrderDetails(price, description, hasdiscount) 
values 
	(10, 'Mouse Logitech X444', 1),
	(1050, 'MSI Laptop', 1),
	(130, 'Display AOC', 0),
	(20, 'Keyboard Logitech Z40', 1)

