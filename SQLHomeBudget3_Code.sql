
create database HomeBudget3

--drop database HomeBudget3


create table wallet(
id_wallet int primary key identity(1,1),
Name_wallet varchar(50),
description_Wallet varchar(100)
)

create table Categories(
id_categories int primary key identity(1,1),
Name_category varchar(50),
description_Catgory varchar(100)
)

create table subcategories(
id_subcategories int primary key identity (1,1),
Name_subcategory varchar(50),
description_Catgory varchar(100),
id_categories int foreign key references Categories(id_categories)
)





create table  client (
id_client int primary key identity(1,1),
firstname varchar(60),
surname varchar(60),
email varchar(50),
Login_Name_Unique varchar(50) unique,
Password_text varchar(50)
)

create table Client_subcategories(
id_Client_subcategories int primary key identity (1,1),
id_subcategories int foreign key references subcategories(id_subcategories),
id_client int foreign key references client (id_client)
)

create table Client_wallet(
id_Client_wallet int primary key identity (1,1),
id_client int foreign key references client (id_client),
id_wallet int foreign key references wallet(id_wallet)
)

create table Transact(
id_trans int primary key identity(1,1),
Amount real,
date_Transaction datetime,
descript varchar(100),
IfIncome bit,
id_client int foreign key references client (id_client),
id_Client_subcategories int foreign key references Client_subcategories(id_Client_subcategories),
id_Client_wallet int foreign key references Client_wallet(id_Client_wallet)
)

insert into wallet
values ('konto bankowe','konto w banku');
insert into wallet
values ('konto oszczêdnoœciowe','Konto oszczêdnoœciowe');
insert into wallet
values ('gotówka','Bilony i banknoty');


select *
from wallet


insert into Categories
values('Wydatki','Kategoria wydatki');
insert into Categories
values('Dochody i wydatki','Kategoria wydatki i dochody');
insert into Categories
values('Dochody','Kategoria dochody');


select *
from Categories

select *
from subcategories


insert into subcategories
values('Pensja','Zarobek ',3);
insert into subcategories
values('Odsetki','Odsetki z konta oszczêdnoœciowego',3);
insert into subcategories
values('Dochód pasywny','Wynajem mieszkania, dywidendy.itp',3);
insert into subcategories
values('Kredyt','Kredyty, po¿yczki .itp',3);
insert into subcategories
values('Prezent','Prezenty, podarunki .itp',3);


insert into subcategories
values('Spo¿ywcze','Jedzenie ',1);
insert into subcategories
values('Papierosy i alkohol','Na³óg ',1);
insert into subcategories
values('Edukacja','Edukacja',1);
insert into subcategories
values('Osobiste','Bilety, fryzjer, zwierzêta itp.',1);
insert into subcategories
values('Media','Pr¹d, woda, gaz itp.',1);
insert into subcategories
values('Rozrywka','Restauracje, kina itp.',1);
insert into subcategories
values('Podró¿e','Wycieczki, muzea itp.',1);
insert into subcategories
values('Sport','Si³ownia, basen itp.',1);
insert into subcategories
values('Zdrowie','Lekarstwa, wizyta u lekarza .itp',1);
insert into subcategories
values('Transport','Paliwo, bilety',1);
insert into subcategories
values('Samochód','OC/AC, serwis itp.',1);

insert into subcategories
values('Wyp³ata gotówki','',2);
insert into subcategories
values('Inwestycje finansowe','Zakup akcji .itp',2);
insert into subcategories
values('Podatki','ZUS, Dochodowy itp.',2);
insert into subcategories
values('Inne','Inne',2);

select *
from subcategories
order by 4 






INSERT INTO client
VALUES ('Krzysztof', 'Klich','kklich97@gmail.com','Kris1', '207023ccb44feb4d7dadca005ce29a64');
INSERT INTO client
VALUES ('Marian', 'Hanowicz','mar@o2.pl','ja','c4ca4238a0b923820dcc509a6f75849b');
insert into client
values ('Hania', 'Ulowicz','ula@o2.pl','Marian234', 'c4ca4238a0b923820dcc509a6f75849b');



insert into Client_subcategories
values(1,1);
insert into Client_subcategories
values(4,1);
insert into Client_subcategories
values(6,1);
insert into Client_subcategories
values(3,1);


insert into Client_subcategories
values(2,1);
insert into Client_subcategories
values(5,1);
insert into Client_subcategories
values(7,1);
insert into Client_subcategories
values(8,1);
insert into Client_subcategories
values(9,1);
insert into Client_subcategories
values(10,1);
insert into Client_subcategories
values(11,1);
insert into Client_subcategories
values(12,1);
insert into Client_subcategories
values(13,1);
insert into Client_subcategories
values(14,1);
insert into Client_subcategories
values(15,1);
insert into Client_subcategories
values(16,1);





insert into Client_subcategories
values(1,2);
insert into Client_subcategories
values(8,2);
insert into Client_subcategories
values(11,2);
insert into Client_subcategories
values(5,3);



select *
from Client_subcategories
order by 3,2




insert into Client_wallet
values(1,1);
insert into Client_wallet
values(2,1);
insert into Client_wallet
values(3,1);
insert into Client_wallet
values(1,2);
insert into Client_wallet
values(2,2);
insert into Client_wallet
values(3,3);
insert into Client_wallet
values(1,3);

select *
from Client_wallet

select *
from Client_subcategories

insert into Transact
values(2500,'2020-03-02','Pieni¹¿ki za luty',1,1,1,1);

insert into Transact
values(1200,'2020-03-12','Praca',1,1,3,4);
insert into Transact
values(69,'2020-02-06','Jedzenie',0,1,4,1);
insert into Transact
values(99,'2020-04-02','Dodatek',1,2,6,2);
insert into Transact
values(156.56,'2020-03-22','Dochod',1,3,8,6);


update Transact
set id_Client_wallet=6
where id_trans=5


select *
from Transact


select t.id_trans, t.Amount,t.descript,t.IfIncome,c.firstname, cs.id_subcategories
from Transact t join client c on t.id_client=c.id_client
join Client_subcategories cs on t.id_Client_subcategories=cs.id_Client_subcategories

select *
from Transact t join client c on t.id_client=c.id_client
join Client_subcategories cs on t.id_Client_subcategories=cs.id_Client_subcategories
join subcategories s on s.id_subcategories=cs.id_subcategories
join Client_wallet cw on cw.id_Client_wallet=t.id_Client_wallet
join wallet w on w.id_wallet=cw.id_wallet
order by 1

select *
from Client_subcategories

select *
from Client_wallet
order by 2

select *
from Client_wallet cw join wallet w on w.id_wallet=cw.id_wallet
join client c on cw.id_client=c.id_client
order by 2

select *
from client


select *
from Transact t join Client_subcategories cs on t.id_Client_subcategories=cs.id_Client_subcategories
join subcategories s on s.id_subcategories=cs.id_subcategories
order by 1

--Dochody
select *
from Client_subcategories cs join subcategories s on s.id_subcategories=cs.id_Client_subcategories
where cs.id_client=1 AND s.id_categories >= 2

--Wydatki
select *
from Client_subcategories cs join subcategories s on s.id_subcategories=cs.id_Client_subcategories
where cs.id_client=1 AND s.id_categories <= 2


select *
from Transact t join Client_wallet cw on t.id_Client_wallet=cw.id_Client_wallet
where cw.id_client=1 AND cw.id_wallet=1


select s.Name_subcategory AS Name_subcategory, sum (t.Amount) as Suma 
from Transact t join Client_subcategories cs on t.id_Client_subcategories=cs.id_Client_subcategories
join subcategories s on cs.id_subcategories=s.id_subcategories
where IfIncome = 1 and cs.id_client=1
group by  s.Name_subcategory
            

select s.Name_subcategory AS Name_subcategory, sum (t.Amount) as Suma 
from Transact t join Client_subcategories cs on t.id_Client_subcategories=cs.id_Client_subcategories
join subcategories s on cs.id_subcategories=s.id_subcategories
where IfIncome = 0 and cs.id_client=1
group by  s.Name_subcategory
          
select w.id_wallet AS id_wallet,w.Name_wallet AS Name_wallet,sum(t.Amount) as Sum 
from Transact t join Client_wallet cw on t.id_Client_wallet=cw.id_Client_wallet
join wallet w on cw.id_wallet=w.id_wallet
where t.id_client =1
group by w.id_wallet,w.Name_wallet









select *
from Client_subcategories

select *
from subcategories

where id_client=1 AND id_subcategories=1
           

		   select w.id_wallet AS id_wallet,w.Name_wallet AS Name_wallet , cw.id_Client_wallet  AS id_Client_wallet 
		   from Client_wallet cw join wallet w on cw.id_wallet = w.id_wallet
		   where cw.id_client = 1
		   group by w.id_wallet,w.Name_wallet ,cw.id_Client_wallet 
		   order by 2
            


select w.id_wallet AS id_wallet,w.Name_wallet AS Name_wallet, sum(t.Amount) as sum
from Client_wallet cw join wallet w on cw.id_wallet = w.id_wallet 
join Transact t on t.id_Client_wallet=cw.id_Client_wallet
where cw.id_client = 1
group by w.id_wallet,w.Name_wallet


select *
from Client_wallet cw join wallet w on w.id_wallet=cw.id_wallet
where cw.id_client=1


select *
from Client_subcategories
where id_client=1


select *
from Transact t join Client_subcategories cs on t.id_Client_subcategories=cs.id_Client_subcategories
where cs.id_client=6

select *
from client

