create or alter procedure scenarioTwo as
begin

	declare @noEntries int = 4;
	declare @base int = 100;
	declare @counter int = @base;

	-- the duplicate contract
	insert into [Contract] values (102, 23311, 'location', SYSDATETIME());

	while @counter < @base + @noEntries
	begin
	begin try
		begin try
			insert into Client values (@counter, 'name' + cast(@counter as varchar(5)));
		end try
		begin catch
			print 'An error occurred: ' + error_message();
			print 'reverting';
		end catch

		set @counter = @counter + 1;


		begin try
			insert into Client values (@counter, 'name' + cast(@counter as varchar(5)));
		end try
		begin catch
			print 'An error occurred: ' + error_message();
			print 'reverting';
		end catch
		
		set @counter = @counter + 1;

		begin try
			insert into [Contract] values (@counter, @counter*1000, 'location'+cast(@counter as varchar(5)), SYSDATETIME());
		end try
		begin catch
			print 'An error occurred: ' + error_message();
			print 'reverting';
		end catch

		set @counter = @counter + 1;

		begin try
			insert into [Contract] values (@counter, @counter*1000, 'location'+cast(@counter as varchar(5)), SYSDATETIME());
		end try
		begin catch
			print 'An error occurred: ' + error_message();
			print 'reverting';
		end catch

		set @counter = @counter + 1;


		insert into Task values (@counter-3, @counter-1)
	end try
	begin catch
	end catch
	end
end

delete from Task where client_key >= 100 and contract_key >= 100
delete from Client where cid >= 100
delete from Contract where cid >= 100

exec scenarioTwo

select * from Client
select * from Contract
select * from Task

use SpaceLaunchCompany