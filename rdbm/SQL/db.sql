


CREATE TABLE Address
    (
        Country VARCHAR(50) NOT NUll,
        PostCode VARCHAR(10) NOT NUll,
        HouseNumber VARCHAR(10) NOT NUll,
        City VARCHAR(50),
        Street VARCHAR(50),
        PRIMARY KEY(Country, PostCode, HouseNumber)
    );

CREATE TABLE HeadQuater(
    BuildingName VARCHAR(50) NOT NULL PRIMARY KEY,
    Rooms INT NOT NULL,
    Rent money NOT NULL,
    Country VARCHAR(50) NOT NUll,
    PostCode VARCHAR(10) NOT NUll,
    HouseNumber VARCHAR(10) NOT NUll,
    CONSTRAINT HeadQuater_Address FOREIGN KEY (Country, PostCode, HouseNumber) REFERENCES Address (Country,PostCode,HouseNumber)
);

CREATE TABLE Employee
    (
        BSN int PRIMARY KEY NOT NUll,
        Name VARCHAR(100) NOT NULL,
        SurName VARCHAR(100) NOT NUll,
        BuildingName VARCHAR(50) NOT NULL,
        CONSTRAINT Employee_HeadQuater FOREIGN KEY (BuildingName) REFERENCES HeadQuater (BuildingName)
    );

CREATE TABLE EmployeeAddress
    (
        BSN int NOT NUll,
        Country VARCHAR(50) NOT NUll,
        PostCode VARCHAR(10) NOT NUll,
        HouseNumber VARCHAR(10) NOT NUll,
        Residence BIT NOT NULL,
        CONSTRAINT Employee_EmployeeAddress FOREIGN KEY (BSN) REFERENCES Employee (BSN),
        CONSTRAINT Address_EmployeeAddress FOREIGN KEY (Country, PostCode, HouseNumber) REFERENCES Address (Country,PostCode,HouseNumber),
        CONSTRAINT EmployeeAddress_PK PRIMARY KEY  (BSN, Country, PostCode, HouseNumber)
    );

--choose key

CREATE TABLE Degree
    (
        Course VARCHAR(150) NOT NUll,
        School VARCHAR(150) NOT NUll,
        Level VARCHAR(100) NOT NULL,
        BSN int Not NULL,
        CONSTRAINT Employee_Degree FOREIGN KEY (BSN) REFERENCES Employee (BSN)
    );



CREATE TABLE Project(
    ProjectID int IDENTITY PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Budget money NOT NULL,
    Hours int NOT NULL,
    BuildingName VARCHAR(50) NOT NULL,
    CONSTRAINT Project_HeadQuater FOREIGN KEY (BuildingName) REFERENCES HeadQuater (BuildingName)
);

CREATE TABLE Position(
    PositionName VARCHAR(100) NOT NUll,
    Description TEXT,
    HourFee money NOT NULL,
    BSN int NOT NULL,
	ProjectID int,
    CONSTRAINT Employee_Position FOREIGN KEY (BSN) REFERENCES Employee (BSN),
	CONSTRAINT Project_Position FOREIGN KEY (ProjectID) REFERENCES Project (ProjectID),
	PRIMARY KEY (BSN, PositionName, ProjectID)
);



