--dirty read
--T2
set transaction isolation level read uncommitted	 --read committed for solution
begin tran
select * from Engine
waitfor delay '00:00:15'
select * from Engine
commit tran
go

--non-repeatable read
--T2
set transaction isolation level read committed
--set transaction isolation level repeatable read	-- solution: set transaction isolation level read committed
begin tran
select * from Engine
waitfor delay '00:00:10'
select * from Engine
commit tran
go

-- phantom read
--T2:
set transaction isolation level repeatable read
-- set transaction isolation level serializable
begin tran
select * from Engine where eid=321
waitfor delay '00:00:10'
select * from Engine where eid=321
commit tran

-- deadlock
--T2:
begin tran
update Engine set thrust=1234 where eid=1
waitfor delay '00:00:10'
update Client set name='deadlock' where cid=1
commit tran