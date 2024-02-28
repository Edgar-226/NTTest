--DB Creation
CREATE TABLE Company (
    company_id CHAR(50) PRIMARY KEY,
    company_name VARCHAR(255)
);


CREATE TABLE Charges  (
    id VARCHAR(50) NOT NULL,
    company_id CHAR(50) NOT NULL,
    amount DECIMAL(30,2) NOT NULL,
    status VARCHAR(30) NOT NULL,
    created_at DATETIME NOT NULL,
    paid_at DATETIME NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (company_id) REFERENCES Company(company_id)
);



SELECT * From Company

SELECT * From Charges


--Consulta para la Vista
SELECT 
    c.company_id As Company_Id,
    c.company_name As Company_Name,
    CONVERT(DATE, ch.paid_at) AS Transaction_Date,
    SUM(ch.amount) AS Total_Amount
FROM 
    Charges ch
JOIN 
    Company c ON ch.company_id = c.company_id
WHERE 
    ch.paid_at IS NOT NULL 
	AND 
	status != 'refunded'
	
GROUP BY 
    c.company_id,
    c.company_name,
    CONVERT(DATE, ch.paid_at);



--View Creation
CREATE VIEW TotalAmountByDayAndCompany AS
SELECT 
    c.company_id As Company_Id,
    c.company_name As Company_Name,
    CONVERT(DATE, ch.paid_at) AS Transaction_Date,
    SUM(ch.amount) AS Total_Amount
FROM 
    Charges ch
JOIN 
    Company c ON ch.company_id = c.company_id
WHERE 
    ch.paid_at IS NOT NULL 
	AND 
	status != 'refunded'
	
GROUP BY 
    c.company_id,
    c.company_name,
    CONVERT(DATE, ch.paid_at);
	
SELECT * FROM TotalAmountByDayAndCompany;


