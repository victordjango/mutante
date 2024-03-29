USE [melimutantedb]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[adn_statistics](
	[mutant_adn_qty] [int] NOT NULL,
	[human_adn_qty] [int] NOT NULL,
	[last_update_date] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[adns]    Script Date: 16/09/2019 2:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[adns](
	[adn_id] [int] IDENTITY(1,1) NOT NULL,
	[adn_request] [varchar](max) NOT NULL,
	[hash_identifier] [varchar](20) NOT NULL,
	[is_mutant] [bit] NOT NULL,
 CONSTRAINT [PK_adns] PRIMARY KEY CLUSTERED 
(
	[adn_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_adns]    Script Date: 16/09/2019 2:20:25 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_adns] ON [dbo].[adns]
(
	[hash_identifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[ADD_DNA_SP]    Script Date: 16/09/2019 2:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
CREATE PROCEDURE [dbo].[ADD_DNA_SP]
	-- Add the parameters for the stored procedure here
	 @dna varchar(max), 
	 @isMutant  bit,
	 @hashcode varchar(20)	
AS
BEGIN
	
	declare @existsDna int

	select @existsDna =  count(*) 
	from adns
	where adns.hash_identifier = @hashcode

	if (@existsDna=0)
	begin
		insert into adns (adn_request, hash_identifier, is_mutant)
		values(@dna, @hashcode,@isMutant)
	end 

END


GO
/****** Object:  StoredProcedure [dbo].[GET_STATS_SP]    Script Date: 16/09/2019 2:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
CREATE PROCEDURE [dbo].[GET_STATS_SP]
	
AS
BEGIN
	
	SELECT st.human_adn_qty, st.mutant_adn_qty
	FROM adn_statistics st

END


GO
/****** Object:  StoredProcedure [dbo].[INIT_DATA_SP]    Script Date: 16/09/2019 2:20:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
CREATE PROCEDURE [dbo].[INIT_DATA_SP]
	
AS
BEGIN
	
	DELETE FROM adns
	DELETE FROM adn_statistics

	INSERT INTO adn_statistics(mutant_adn_qty, human_adn_qty, last_update_date)
	values(0,0,getdate())
		
END
GO

USE [melimutantedb]
GO

/****** Object:  Trigger [dbo].[trg_insert_adns]    Script Date: 16/09/2019 2:26:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[trg_insert_adns]
ON [dbo].[adns]
AFTER INSERT AS
BEGIN

DECLARE @isMutant BIT;

SELECT @isMutant= inserted.is_mutant
FROM inserted

if (@isMutant =1)
BEGIN
	update adn_statistics set mutant_adn_qty = mutant_adn_qty + 1,
	last_update_date = GETDATE()
END
ELSE
BEGIN
	update adn_statistics set human_adn_qty = human_adn_qty + 1
	,last_update_date = GETDATE()
END

END

GO


