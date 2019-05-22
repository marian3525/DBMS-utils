create or alter procedure scenarioOne as
begin
	declare @addedClients table(idx int, id int);
	declare @addedContracts table(idx int, id int);

	declare @noEntries int = 8;
	declare @base int = 100;
	declare @counter int = @base;

	-- the duplicate contract that will cause an error
	insert into [Contract] values (102, 1234, 'location', sysdatetime());

	while @counter < @base + @noEntries
	begin
		begin try
			insert into Client values (@counter, 'name' + cast(@counter as varchar(5)));
			insert into @addedClients values (@counter, @counter);
			set @counter = @counter + 1;
			
			insert into Client values (@counter, 'name' + cast(@counter as varchar(5)));
			insert into @addedClients values (@counter, @counter);
			set @counter = @counter + 1;

			insert into [Contract] values (@counter, @counter*1000, 'location'+cast(@counter as varchar(5)), SYSDATETIME());
			insert into @addedContracts values (@counter, @counter);
			set @counter = @counter + 1;

			insert into [Contract] values (@counter, @counter*1000, 'location'+cast(@counter as varchar(5)), SYSDATETIME());
			insert into @addedContracts values (@counter, @counter);
			set @counter = @counter + 1;

			insert into Task values (@counter-4, @counter-2);
		end try
		begin catch
			print 'An error occurred: ' + error_message();
			
			delete from Task where client_key in (select id from @addedClients) and
									contract_key in (select id from @addedContracts)
			delete from [Contract] where cid in (select id from @addedContracts)
			delete from Client where cid in (select id from @addedClients)
			return;
		end catch
	end
end

exec scenarioOne

delete from Task where client_key >= 100 and contract_key >= 100
delete from Client where cid >=100
delete from Contract where cid >= 100

select * from Client
select * from Contract
select * from Task

use SpaceLaunchCompany