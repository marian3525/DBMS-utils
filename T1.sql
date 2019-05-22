use SpaceLaunchCompany
go
-- dirty read
--T1
begin transaction
update Engine set thrust=1000 where eid=2
waitfor delay '00:00:10';
rollback transaction
go

--non-repeatable reads
--T1:
begin tran
waitfor delay '00:00:10'
update Engine set fueltype='non-repeatable' where eid=2
--update Engine set fueltype='kerosene' where fueltype='non-repeatable'
commit tran

--phantom read
--T1
begin tran
waitfor delay '00:00:10'
insert into Engine values (321, 1234, 'phantom_fuel', 1)
commit tran

delete from Engine where eid=321

-- deadlock
-- T1:
begin tran
set deadlock_priority high
update Client set name='deadlock' where cid=1 --Client locked
waitfor delay '00:00:10'
update Engine set thrust=1111 where eid=1	-- Engine locked
commit tran