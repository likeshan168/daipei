.sql--促销信息表
create table promotions
(
	p_id int primary key  not null identity(1,1),--编号
	p_content varchar(1000) not null,--促销的内容
	remark varchar(1000)--备注
)
go


--店铺场地图表
create table areapic
(
	id int identity(1,1) primary key not null,
	dpid varchar(30) not null,--店铺id
	pic_name varchar(50) not null,--图片名称
	--pic_data varbinary(max) not null,--图片数据
	remark varchar(1000)--备注
)
go

--记录异常的表
CREATE TABLE [dbo].[ExceptionLog] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [ErrorDate]     NVARCHAR (50)   NOT NULL,
    [ExceptionType] NVARCHAR (1000) NOT NULL,
    [Message]       NVARCHAR (1024) NOT NULL,
    [StackTrace]    NTEXT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
go
--记录上货日期的表
CREATE TABLE [dbo].[Style_EditionHandle] (
    [StyleCode]     VARCHAR (50)   NOT NULL,--款式+颜色
    [EditionHandle] VARCHAR (50)   NULL,--上市日期
	[p_id] int, --foreign key references promotions(p_id),--促销id
    [Remark]        VARCHAR (1000) NULL,--备注
    PRIMARY KEY CLUSTERED ([StyleCode] ASC)
);
--保存推荐的表
CREATE TABLE [dbo].[tjdp] (
    [id]        INT            IDENTITY (1, 1) NOT NULL,--序号
    [use_id]    VARCHAR (30)   NOT NULL,--店铺id
    [cbpicture] VARCHAR (1000) NOT NULL,--组合图id
    [tjdate]    VARCHAR (30)   NOT NULL,--推荐日期
    [mqu]       VARCHAR (10)   DEFAULT (NULL) NULL,--主打款的库存
    [lqu1]      VARCHAR (10)   DEFAULT (NULL) NULL,--内搭1库存
    [lqu2]      VARCHAR (10)   DEFAULT (NULL) NULL,--内搭2库存
    [bqu1]      VARCHAR (10)   DEFAULT (NULL) NULL,--下身1库存
    [bqu2]      VARCHAR (10)   DEFAULT (NULL) NULL,--下身2库存
    [aqu1]      VARCHAR (10)   DEFAULT (NULL) NULL,--配饰1库存
    [aqu2]      VARCHAR (10)   DEFAULT (NULL) NULL,--配饰2库存
	[remark]    VARCHAR (100)  DEFAULT (NULL) NULL--备注
    PRIMARY KEY CLUSTERED ([id] ASC)
);
go

go
--出货视图1
CREATE VIEW [dbo].[chuhuo1] 
AS  
	Select Dh,Use_out,Use_ent,Inp_dt,Unt_pr,Cos_pr,Com_pr,Com_qu,
	out_nm,ent_nm,Com_id,Siz_dr,col_no,Col_dr,Dyan_no Sty_no,
	Com_nm,Use_no,Grp_id,Siz_id ,'Y' Sanc from eissy.dbo.DyantoStandSty 
	inner join 
	eissy.dbo.vwC103 on DyantoStandSty.Sty_no=vwC103.sty_no 
	Inner Join 
	eissy.dbo.Fila11e On Fila11e.Use_id=vwC103.USe_Ent 
	Inner Join 
	eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=vwC103.Use_Out  
	Where 1=1 And fila11e.user_id='LFL' And FIla11e1.User_id='LFL';
go

--出货视图2
CREATE VIEW [dbo].[chuhuo2] 
AS 
	Select Dh,Use_out,Use_ent,Inp_dt,Unt_pr,Cos_pr,Com_pr,Com_qu,out_nm,ent_nm,
	Com_id,Siz_dr,col_no,Col_dr,Dyan_no Sty_no,Com_nm,Use_no,Grp_id,Siz_id ,'N' Sanc 
	from eissy.dbo.DyantoStandSty 
	Inner join 
	eissy.dbo.vwCDraft103 on DyantoStandSty.sty_no=vwCDraft103.sty_no 
	Inner Join 
	eissy.dbo.Fila11e On Fila11e.Use_id=vwCDraft103.Use_Ent 
	Inner Join 
	eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=vwCDraft103.Use_Out  
	Where 1=1 And fila11e.user_id='LFL' And FIla11e1.User_id='LFL';
go

--店铺视图
CREATE VIEW [dbo].[dianpu] 
AS  
	Select distinct Fila01a.Use_id,Use_nm From eissy.dbo.Fila01a 
	Inner Join 
	eissy.dbo.Fila11e on Fila01a.Use_id=Fila11e.Use_id 
	Where User_id='lfl' and Del_bz='N' 
	And Use_LB <> '6' And Fila11e.User_id='lfl' --order by Use_nm
go

--库存视图(这个是真实的库存)
--2014-09-03 加入虚拟库存就是加入了出货申请
--CREATE VIEW [dbo].[kucun] AS
--	SELECT vwS003.Use_id,Use_nm,Com_nm,Sty_no,Col_no,Col_dr,Siz_dr,Unt_pr,Cos_pr,
--	convert(decimal(10,2),vws003.Sto_n1) Com_qu,Cos_pr*vws003.Sto_n1 Cos_Sum,
--	Unt_pr*vws003.Sto_n1 Unt_Sum,Grp_id,Siz_id FROM eissy.dbo.vwS003 
--	Where Del_bz<>'Y';
--go


--虚拟库存
create view [dbo].[kucun]
as
	SELECT vwS003.Use_id,Use_nm,Com_nm,Sty_no,Col_no,Col_dr,Siz_dr,Unt_pr,Cos_pr,
	convert(decimal(10,2),vws003.Sto_n1) Com_qu,Cos_pr*vws003.Sto_n1 Cos_Sum,
	Unt_pr*vws003.Sto_n1 Unt_Sum,Grp_id,Siz_id FROM eissy.dbo.vwS003 
	Where Del_bz<>'Y'
	union
	SELECT use_ent,ent_nm,Com_nm,Dyan_no Sty_no,col_no,Col_dr,Siz_dr,Unt_pr,Cos_pr,
	convert(decimal(10,2),Com_qu) Com_qu,Cos_pr*Com_qu Cos_Sum,
	Unt_pr*Com_qu Unt_Sum,Grp_id,Siz_id 
	FROM eissy.dbo.DyantoStandSty 
	inner join 
	eissy.dbo.vwC403 
	on dyantoStandSty.sty_no=vwC403.Sty_no 
	Inner Join 
	eissy.dbo.Fila11e 
	On Fila11e.Use_Id=vwc403.Use_Ent  
	Inner Join 
	eissy.dbo.Fila11e Fila11e1 
	On Fila11e1.Use_id=vwC403.Use_Out  
	Where 1=1 --And Inp_dt>='2013-02-23' And Inp_dt<='2014-08-22' 
	And Com_id Like '%' and  fila11e.User_id='zzr'  
	And Fila11e1.user_id='zzr'  
go



--入库视图1
CREATE VIEW [dbo].[ruku1] 
AS 
	Select Dh,Use_out,Use_ent,Inp_dt,Unt_pr,Cos_pr,Com_pr,
	Com_qu,out_nm,ent_nm,Com_id,Siz_dr,col_no,Col_dr,
	Dyan_no Sty_no,Com_nm,Grp_id,Siz_id ,
	'Y' Sanc from eissy.dbo.DyantoStandSty 
	inner join 
	eissy.dbo.vwb005 on DyantoStandSty.Sty_no=vwb005.sty_no
	Inner Join 
	eissy.dbo.Fila11e On Fila11e.Use_id=vwb005.USe_Ent
	Inner Join 
	eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=vwb005.Use_Out  
	Where 1=1 And fila11e.user_id='LFL' And FIla11e1.User_id='LFL'
go
--入库视图2
CREATE VIEW [dbo].[ruku2] 
AS 
	Select Dh,Use_out,Use_ent,Inp_dt,
	Unt_pr,Cos_pr,Com_pr,Com_qu,out_nm,
	ent_nm,Com_id,Siz_dr,col_no,Col_dr,
	Dyan_no Sty_no,Com_nm,Grp_id,Siz_id ,
	'N' Sanc from eissy.dbo.DyantoStandSty 
	Inner join 
	eissy.dbo.vwBDraft005 on DyantoStandSty.sty_no=vwBDraft005.sty_no 
	Inner Join 
	eissy.dbo.Fila11e On Fila11e.Use_id=vwBDraft005.Use_Ent 
	Inner Join 
	eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=vwBDraft005.Use_Out  
	Where 1=1 And fila11e.user_id='LFL' And FIla11e1.User_id='LFL'
go

--款式视图
CREATE VIEW [dbo].[style] 
AS 
	select distinct replace(B.sty_no,'','') sty_no,
	replace(B.col_no,'','') col_no ,A.Com_nm,B.Unt_pr 
	from eissy.dbo.FilA03B B
	left join 
	eissy.dbo.FiLA03A A on B.Sty_no=A.Sty_no;
go


 --销售视图
CREATE VIEW [dbo].[xiaoshou] 
AS SELECT 
	sty_no,Com_nm,Col_no,Col_dr,Siz_no,Siz_dr,
	Cos_Pr Avg_Cos,Unt_Pr Avg_Unt,Cos_pr*Com_qu ttl_Cos_pr,
	Unt_pr*Com_qu ttl_Unt_pr,Com_qu Com_qu,Com_pr Com_pr,
	Grp_id,Siz_id,Use_ID,Sal_Dt From eissy.dbo.VWNQ001;
go
--up4软件中的图片数据
CREATE VIEW [dbo].[Up4Image]
AS 
	select st.Code,si.StyleImage from 
	opendatasource('sqloledb','data source=www.cosen168.com;user id=sa;password=cosen').eissy_up4.dbo.SM_StyleImage si 
	left join 
	opendatasource('sqloledb','data source=www.cosen168.com;user id=sa;password=cosen').eissy_up4.dbo.SM_Style st 
	on si.StyleID=st.ID
go


--这个是获取出货，调拨，退货各个店铺明细的存储过程（为导出excel报表用的）
--这个还是用于报表当中
CREATE proc [dbo].[getchdbdetail]
	@startdate varchar(30),
	@enddate varchar(30),
	@cond varchar(1000)
as


	declare @tmp varchar(1000)
	set @tmp=dbo.concatsql(@cond)
	exec('select ent_nm,replace(sty_no,'''','''')+replace(col_no,'''','''') style,com_qu from '+
	'chuhuo(''1'','''+@startdate+''','''+@enddate+''') where use_ent in '+@tmp+'')

	exec('select ent_nm,replace(sty_no,'''','''')+replace(col_no,'''','''') style,com_qu from '+ 
	'diaobo(''1'','''+@startdate+''','''+@enddate+''') where use_ent in '+@tmp+'')

	exec('select out_nm,replace(sty_no,'''','''')+replace(col_no,'''','''') style,com_qu from '+ 
	'tuihuo(''1'','''+@startdate+''','''+@enddate+''') where use_ent in  '+@tmp+'')

	--select ent_nm,replace(sty_no,'','')+replace(col_no,'','') style,com_qu from 
	--chuhuo('1',@startdate,@enddate,@cond) 

	--select ent_nm,replace(sty_no,'','')+replace(col_no,'','') style,com_qu from  
	--diaobo('1',@startdate,@enddate,@cond)

	--select out_nm,replace(sty_no,'','')+replace(col_no,'','') style,com_qu from 
	--tuihuo('1',@startdate,@enddate,@cond)
go


--获取所有的搭配信息（这个不是推荐的搭配，而是所有的搭配，推荐搭配就是从这里开始）
--加入了未来库存的概念(注意现在未来库存的概念是：现在的库存+出货申请单（所有的，审核和未审核）)
CREATE PROCEDURE [dbo].[getdp]
(
	@uid varchar(30),
	@dt1 varchar(30),
	@dt2 varchar(30),
	@of int
)
as 
	select * from getdpnopage_jiameng(@uid,@dt1,@dt2) order by  masterstyle,mqu desc 
	if @of=0 
		begin
			select count(*) from tjdp  where use_id=@uid and tjdate=@dt2;
			select distinct top 10  tjdate from tjdp where use_id=@uid order by tjdate desc;
		end
go

--获取某个主打款的搭配信息
CREATE proc [dbo].[GetDpByStyleNo_Proc]
@StyleNo varchar(50)
as
	select MasterImg,Legging1,LeggingImg1,Legging2,
	LeggingImg2,Bottom1,BottomImg1,Bottom2,
	BottomImg2,Accessories1,AccessoriesImg1 ,
	Accessories2,AccessoriesImg2, 
	CbPicture from dbo.CollocationNew 
	where REPLACE(MasterStyle,'','')=@StyleNo 
go

--报表(报表就只需要看出货，也要看出货申请)

CREATE  proc [dbo].[report]
(
	@sltType varchar(10),--按入1：库时间查询还是2：上市日期查询
	@dt1 varchar(20), 
	@dt2 varchar(20),
	@cond varchar(1000)--店铺
)
as
begin
	CREATE Table #xstmp
	(
		sty_no varchar(30),
		col_no varchar(30),
		com_qu decimal(10,2), 
		com_pr decimal(10,2)
	);
	insert into  #xstmp
	EXEC  xiaoshoup @sltType, @dt1,@dt2,@cond
	declare @tmp varchar(1000);
	declare @tmpsql varchar(5000)
	set @tmp=dbo.concatsql(@cond);

	if @sltType='1' --按入库日期进行查询
		begin
			exec('select a.nsty_no,f.com_nm,sm.editionhandle,sm.p_id,isnull(a.num,0) as rknum,
			isnull(b.num,0) as chnum,isnull(db.num,0) as dbnum,
			isnull(th.num,0) as thnum,
			isnull(c.num,0) as xsnum,isnull(d.num,0) as cknum,
			isnull(cast(f.money as decimal(10,2)),0) as unitprice,
			isnull(cast(c.money as decimal(10,2)),0) as xsmoney
			from(select sty_no+col_no nsty_no,sum(com_qu) as num  
			from ruku('''+@sltType+''','''+@dt1+''','''+@dt2+''') group by sty_no+col_no) as a 
			left join (select sty_no+col_no nsty_no , com_nm,unt_pr as money from style) as f 
			on a.nsty_no=f.nsty_no
			left join(select sty_no+col_no nsty_no, sum(com_qu) as num from 
			chuhuo('''+@sltType+''','''+@dt1+''','''+@dt2+''') where use_ent in('+@tmp+')  
			group by sty_no+col_no) as b on a.nsty_no= b.nsty_no 
			left join(select sty_no+col_no nsty_no, sum(com_qu) as num from 
			diaobo('''+@sltType+''','''+@dt1+''','''+@dt2+''') where use_ent in('+@tmp+') 
			group by sty_no+col_no) as db on a.nsty_no= db.nsty_no  
			left join(select sty_no+col_no nsty_no, sum(com_qu) as num from 
			tuihuo('''+@sltType+''','''+@dt1+''','''+@dt2+''')  where use_out in('+@tmp+')
			group by sty_no+col_no) as th on a.nsty_no= th.nsty_no 
			left join(select nsty_no,com_qu as num,
			com_qu*com_pr as money from (SELECT sty_no+col_no nsty_no,
			Sum(com_qu) com_qu,Sum(com_pr) com_pr From #xstmp 
			Group By sty_no+col_no  ) tp) as c on replace(c.nsty_no,'''','''')=a.nsty_no
			left join(select sty_no+col_no nstyl_no,sum(com_qu) as num from kucun where use_id in ('+@tmp+')
			group by sty_no+col_no) as d on replace(d.nstyl_no,'''','''')=a.nsty_no
			left join style_editionhandle as sm on a.nsty_no=sm.stylecode')
			
		end
	else--按上市日期进行查询
		begin
			exec('select a.nsty_no,f.com_nm,sm.editionhandle,sm.p_id,isnull(a.num,0) as rknum,
			isnull(b.num,0) as chnum,isnull(db.num,0) as dbnum,
			isnull(th.num,0) as thnum,
			isnull(c.num,0) as xsnum,isnull(d.num,0) as cknum,
			isnull(cast(f.money as decimal(10,2)),0) as unitprice,
			isnull(cast(c.money as decimal(10,2)),0) as xsmoney
			from(select sty_no+col_no nsty_no,sum(com_qu) as num  
			from ruku('''+@sltType+''','''+@dt1+''','''+@dt2+''') group by sty_no+col_no) as a 
			left join (select sty_no+col_no nsty_no , com_nm,unt_pr as money from style) as f 
			on a.nsty_no=f.nsty_no
			left join(select sty_no+col_no nsty_no, sum(com_qu) as num from 
			chuhuo('''+@sltType+''','''+@dt1+''','''+@dt2+''')  where use_ent in('+@tmp+')
			group by sty_no+col_no) as b on a.nsty_no= b.nsty_no 
			left join(select sty_no+col_no nsty_no, sum(com_qu) as num from 
			diaobo('''+@sltType+''','''+@dt1+''','''+@dt2+''')  where use_ent in('+@tmp+')
			group by sty_no+col_no) as db on a.nsty_no= db.nsty_no  
			left join(select sty_no+col_no nsty_no, sum(com_qu) as num from 
			tuihuo('''+@sltType+''','''+@dt1+''','''+@dt2+''')  where use_out in('+@tmp+')
			group by sty_no+col_no) as th on a.nsty_no= th.nsty_no 
			left join(select nsty_no,com_qu as num,
			com_qu*com_pr as money from (SELECT sty_no+col_no nsty_no,
			Sum(com_qu) com_qu,Sum(com_pr) com_pr From #xstmp 
			Group By sty_no+col_no  ) tp) as c on replace(c.nsty_no,'''','''')=a.nsty_no
			left join(select sty_no+col_no nstyl_no,sum(com_qu) as num from kucun where use_id in ('+@tmp+')
			group by sty_no+col_no) as d on replace(d.nstyl_no,'''','''')=a.nsty_no
			right join style_editionhandle as sm on a.nsty_no=sm.stylecode  where sm.EditionHandle>='''+@dt1+''' and sm.EditionHandle<='''+@dt2+''' ')
			
		end
	exec(@tmpsql)
	drop table #xstmp
end
go
--销售存储过程
CREATE  proc [dbo].[xiaoshoup]
(
	 @sltType varchar(10),--如果是按上市日期查询那么就不需要时间限制
	 @dt1 varchar(20), 
	 @dt2 varchar(20),
	 @cond varchar(1000)
)

as
begin
	declare @tmpsql varchar(5000)
	if @sltType='1' --按入库时间查询
		begin
			set @tmpsql='select  sty_no,col_no,com_qu,com_pr from xiaoshou where Sal_Dt>='''+@dt1+''' and sal_dt<='''+@dt2+''' and use_id in ('
		end
	else--按上市日期查询
		begin
			set @tmpsql='select  sty_no,col_no,com_qu,com_pr from xiaoshou where  use_id in ('
		end
	declare @next int  
	declare @ids varchar(20)
	set @ids=''
	set @next=1
	--while @next<=eissydp.dbo.splitlen(@cond,',')
	while @next<=eissy.dbo.splitlen(@cond,',')
		begin
			--set @ids=eissydp.dbo.mysplit(@cond,',',@next) 
			set @ids=eissy.dbo.mysplit(@cond,',',@next) 
			set @tmpsql=@tmpsql+''''+@ids+''''+','
			set @next=@next+1
		end
		set @tmpsql=substring(@tmpsql,0,len(@tmpsql))+');'
		exec(@tmpsql)
end
go

--出货函数
create  function [dbo].[chuhuo]
(
	 @sltType varchar(10),
	 @dt1 varchar(20), 
	 @dt2 varchar(20)
	 --@dpids varchar(4000)
)
returns @chuhuo table
(
	ent_nm varchar(50),
	use_ent varchar(50),
	inp_dt varchar(30),
	com_qu decimal(10,2),
	col_no varchar(30),
	sty_no varchar(30)
)
as
--return
begin
	--declare @tmp varchar(4000);
	--set @tmp=dbo.concatsql(@dpids);
	if @sltType='1'
		begin
			--SELECT Dh,Use_out,Use_ent,Inp_dt,convert(decimal(10,2),Unt_pr) as Unt_pr,
			--convert(decimal(10,2),Cos_pr) as Cos_pr,convert(decimal(10,2),Com_pr) as Com_pr,
			--convert(decimal(10,2),Com_qu) as Com_qu,out_nm,ent_nm,
			--Com_id,Siz_dr,col_no,Col_dr,Sty_no,Com_nm,Use_no,Grp_id,Siz_id ,
			--Sanc FROM
			--(
			--select *from chuhuo1 where Inp_dt>=@dt1 
			--And Inp_dt<=@dt2 union all
			--select *from chuhuo2 where Inp_dt>=@dt1 And Inp_dt<=@dt2
			--) a
			insert into @chuhuo
			select * from
			(
				select ent_nm,Use_ent,Inp_dt,convert(decimal(10,2),Com_qu) as Com_qu,col_no,Sty_no 
				from chuhuo1 
				where Inp_dt>=@dt1 And Inp_dt<=@dt2 
				--and use_ent in (@tmp)
				union all
				select ent_nm,Use_ent,Inp_dt,convert(decimal(10,2),Com_qu) as Com_qu,col_no,Sty_no
				from chuhuo2 
				where Inp_dt>=@dt1 And Inp_dt<=@dt2 
				--and use_ent in(@tmp)
			) a 
		end
	else
		begin
			insert into @chuhuo
			select * from
			(
				select ent_nm,Use_ent,Inp_dt,convert(decimal(10,2),Com_qu) as Com_qu,col_no,Sty_no 
				from chuhuo1 
				--where use_ent in(@tmp)
				union all
				select ent_nm,Use_ent,Inp_dt,convert(decimal(10,2),Com_qu) as Com_qu,col_no,Sty_no
				from chuhuo2 
				--where use_ent in(@tmp)
			) a 
		end
	return 
end
go


--出货申请视图
create view [dbo].[chsqview]
as
	
		SELECT out_Nm,ent_Nm,col_no,Unt_pr,Cos_pr,Com_pr,
		Com_qu,Chd_qu, Com_id,Col_dr,Siz_dr,Dh,Use_ent,
		Inp_dt,Com_nm,Dyan_no Sty_no,Use_out,Chd_bz, 
		Vis_bz,Status,Grp_id,Siz_id 
		FROM eissy.dbo.DyantoStandSty 
		inner join 
		eissy.dbo.vwC403 
		on dyantoStandSty.sty_no=vwC403.Sty_no 
		Inner Join 
		eissy.dbo.Fila11e 
		On Fila11e.Use_Id=vwc403.Use_Ent  
		Inner Join 
		eissy.dbo.Fila11e Fila11e1 
		On Fila11e1.Use_id=vwC403.Use_Out  
		Where 1=1 --And Inp_dt>='2013-02-23' And Inp_dt<='2014-08-22' 
		And Com_id Like '%' and  fila11e.User_id='zzr'  
		And Fila11e1.user_id='zzr'  
		--ORDER BY Dh,Com_id
	
go

--出货申请函数
create  function [dbo].[chuhuosq]
(
	 @sltType varchar(10),
	 @dt1 varchar(20), 
	 @dt2 varchar(20)
	 --@dpids varchar(4000)
)
returns @chuhuosq table
(
	ent_nm varchar(50),
	use_ent varchar(50),
	inp_dt varchar(30),
	com_qu decimal(10,2),
	col_no varchar(30),
	sty_no varchar(30)
)
as
--return
begin
	if @sltType='1'
		begin
			insert into @chuhuosq
			select * from
			(
				select ent_nm,Use_ent,Inp_dt,convert(decimal(10,2),Com_qu) as Com_qu,col_no,Sty_no 
				from chsqview
				where Inp_dt>=@dt1 And Inp_dt<=@dt2 
			) a 
		end
	else
		begin
			insert into @chuhuosq
			select * from
			(
				select ent_nm,Use_ent,Inp_dt,convert(decimal(10,2),Com_qu) as Com_qu,col_no,Sty_no 
				from chsqview 
				
			) a 
		end
	return 
end
go

--调拨函数
CREATE  function [dbo].[diaobo]
(
 --@dpid varchar(20),
	@sltType varchar(10),
	@dt1 varchar(20), 
	@dt2 varchar(20)
	--@dpids varchar(4000)
)
returns @diaobo table
(
	ent_nm varchar(50),
	use_ent varchar(50),
	inp_dt varchar(30),
	sty_no varchar(30),
	col_no varchar(30),
	com_qu decimal(10,2)
)
as
	begin
		--SELECT Dh,Use_out,Use_ent,Inp_dt,Com_id,Sty_no,Com_nm,col_no,
		--Col_dr,Siz_dr,Out_nm,Ent_nm,Unt_pr,Cos_pr,Com_pr,
		--convert(decimal(10,2),Com_qu) as Com_qu,Use_no,Grp_id,Siz_id,Sanc 
		--FROM (Select Dh,Use_out,Use_ent,Inp_dt,Com_id,Dyan_no Sty_no,
		--Com_nm,col_no,Col_dr,Siz_dr,Out_nm,Ent_nm,Unt_pr,Cos_pr,Com_pr,
		--Com_qu,Use_no,Grp_id,Siz_id,'Y' Sanc From eissy.dbo.DyantoStandSty 
		--inner join 
		--eissy.dbo.vwC203 on eissy.dbo.DyantoStandSty.sty_no=eissy.dbo.vwC203.sty_No
		--Inner Join 
		--eissy.dbo.Fila11e On Fila11e.Use_id=eissy.dbo.vwC203.Use_Ent 
		--Inner Join 
		--eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id = eissy.dbo.vwC203.Use_Out  
		--Where 1=1 And Inp_dt>=@dt1 And Inp_dt<=@dt2 And Com_id Like '%'
		--And fila11e.user_id='lfl' And FIla11e1.User_id='lfl' 
		--Union All Select Dh,Use_out,Use_ent,Inp_dt,Com_id,Dyan_no Sty_no,
		--Com_nm,col_no,Col_dr,Siz_dr,Out_nm,Ent_nm,Unt_pr,Cos_pr,Com_pr,
		--convert(decimal(10,2),Com_qu) as Com_qu,Use_no,Grp_id,Siz_id,'N' Sanc 
		--from eissy.dbo.DyantoStandSty 
		--inner join 
		--eissy.dbo.vwcdraft203 on eissy.dbo.DyantoStandSty.sty_no=eissy.dbo.vwCdraft203.sty_no
		--Inner Join eissy.dbo.Fila11e On Fila11e.Use_id=eissy.dbo.vwCdraft203.Use_ent 
		--Inner Join eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=eissy.dbo.vwCdraft203.Use_out 
		--Where 1=1 And Inp_dt>=@dt1 And Inp_dt<=@dt2 And Com_id Like '%'
		--And fila11e.user_id='lfl' And FIla11e1.User_id='lfl' ) A  --ORDER BY Dh,Com_id 
		--declare @tmp varchar(4000);
		--set @tmp=dbo.concatsql(@dpids);
		if @sltType='1'
			begin
				insert into @diaobo
				select * from 
				(
					select ent_nm,use_ent,inp_dt,Dyan_no Sty_no,col_no,
					convert(decimal(10,2),Com_qu) as Com_qu  From eissy.dbo.DyantoStandSty 
					inner join 
					eissy.dbo.vwC203 on eissy.dbo.DyantoStandSty.sty_no=eissy.dbo.vwC203.sty_No
					Inner Join 
					eissy.dbo.Fila11e On Fila11e.Use_id=eissy.dbo.vwC203.Use_Ent 
					Inner Join 
					eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id = eissy.dbo.vwC203.Use_Out  
					Where 1=1 And Inp_dt>=@dt1 And Inp_dt<=@dt2
					And Com_id Like '%'
					And fila11e.user_id='lfl' And FIla11e1.User_id='lfl' 
					--and use_ent in (@tmp)
					Union All 
					Select ent_nm,use_ent,inp_dt,Dyan_no Sty_no,col_no,
					convert(decimal(10,2),Com_qu) as Com_qu  
					from eissy.dbo.DyantoStandSty 
					inner join 
					eissy.dbo.vwcdraft203 on eissy.dbo.DyantoStandSty.sty_no=eissy.dbo.vwCdraft203.sty_no
					Inner Join eissy.dbo.Fila11e On Fila11e.Use_id=eissy.dbo.vwCdraft203.Use_ent 
					Inner Join eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=eissy.dbo.vwCdraft203.Use_out 
					Where 1=1 And Inp_dt>=@dt1 And Inp_dt<=@dt2 And Com_id Like '%'
					And fila11e.user_id='lfl' And FIla11e1.User_id='lfl' 
					--and use_ent in (@tmp)
				) A   
			end
		else
			begin
				insert into @diaobo
				select * from 
				(
					select ent_nm,use_ent,inp_dt,Dyan_no Sty_no,col_no,
					convert(decimal(10,2),Com_qu) as Com_qu  From eissy.dbo.DyantoStandSty 
					inner join 
					eissy.dbo.vwC203 on eissy.dbo.DyantoStandSty.sty_no=eissy.dbo.vwC203.sty_No
					Inner Join 
					eissy.dbo.Fila11e On Fila11e.Use_id=eissy.dbo.vwC203.Use_Ent 
					Inner Join 
					eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id = eissy.dbo.vwC203.Use_Out  
					Where 1=1 
					And Com_id Like '%'
					And fila11e.user_id='lfl' And FIla11e1.User_id='lfl' 
					--and use_ent in (@tmp)
					Union All 
					select ent_nm,use_ent,inp_dt,Dyan_no Sty_no,col_no,
					convert(decimal(10,2),Com_qu) as Com_qu 
					from eissy.dbo.DyantoStandSty 
					inner join 
					eissy.dbo.vwcdraft203 on eissy.dbo.DyantoStandSty.sty_no=eissy.dbo.vwCdraft203.sty_no
					Inner Join eissy.dbo.Fila11e On Fila11e.Use_id=eissy.dbo.vwCdraft203.Use_ent 
					Inner Join eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=eissy.dbo.vwCdraft203.Use_out 
					Where 1=1
					And Com_id Like '%'
					And fila11e.user_id='lfl' And FIla11e1.User_id='lfl' 
					--and use_ent in (@tmp)
				) A  

			end
				
		return 
	end
go



--表示加入了未来库存(现在的未来库存计算方法=现在库存+出货申请)的功能和上市日期的功能
--这个是获取数据库中所有的搭配组合
CREATE  function [dbo].[getdpnopage_jiameng]
(
	 @uid varchar(20), 
	 @dt1 varchar(20),
	 @dt2 varchar(20)
 --@flag int
)
returns table
as
return
	select m.com_qu mqu, m.masterstyle,m.mdate,
	isnull(l1.com_qu,0) lqu1,m.legging1,isnull(l1.datel1,'') datel1,
	isnull(l2.com_qu,0) lqu2,m.legging2,isnull(l2.datel2,'') datel2,
	isnull(b1.com_qu,0) bqu1,m.bottom1,isnull(b1.dateb1,'') dateb1,
	isnull(b2.com_qu,0) bqu2,m.bottom2,isnull(b2.dateb2,'') dateb2,
	isnull(a1.com_qu,0) aqu1,m.accessory1,isnull(a1.datea1,'') datea1,
	isnull(a2.com_qu,0) aqu2,m.accessory2,isnull(a2.datea2,'') datea2,
	m.cbpicture,t.cbpicture isdp

--主打款信息
from(
	select k.com_qu ,k.EditionHandle mdate,d.masterstyle,d.type,d.legging1,d.legging2,
	d.bottom1,d.bottom2,d.accessory1,d.accessory2,d.cbpicture 
	from(
		select kc.styleno,kc.com_qu+isnull(ch.com_qu,0) as com_qu,sm.EditionHandle from 
		(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
			sum(cast(com_qu as decimal(10,0))) com_qu 
			from kucun where use_id=@uid  and com_qu>0 
			group by replace(sty_no,'','')+replace(col_no,'','')) kc
		left join--加入出货（计算未来库存）
		(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
			sum(cast(com_qu as decimal(10,0))) com_qu 
			from chuhuosq('1',@dt1,@dt2) where Use_ent=@uid
			group by replace(sty_no,'','')+replace(col_no,'','')) ch
			on kc.styleno=ch.styleno
		--加入上市日期
		left join style_editionhandle sm
			on sm.StyleCode=kc.styleno
		) as k inner join 
	dapei d on k.styleno=d.masterstyle 
) m
--内搭1信息
left join
(
select kc.styleno,kc.com_qu+isnull(ch.com_qu,0) as com_qu,sm.EditionHandle datel1 from 
	(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
		sum(cast(com_qu as decimal(10,0))) com_qu 
		from kucun where use_id=@uid  and com_qu>0 
		group by replace(sty_no,'','')+replace(col_no,'','')) kc
	left join
	(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
		sum(cast(com_qu as decimal(10,0))) com_qu 
		from chuhuosq('1',@dt1,@dt2) where Use_ent=@uid
		group by replace(sty_no,'','')+replace(col_no,'','')) ch
		on kc.styleno=ch.styleno
	--加入上市日期
	left join style_editionhandle sm
		on sm.StyleCode=kc.styleno
) l1 on l1.styleno=m.legging1
--内搭2信息
left join
(
select kc.styleno,kc.com_qu+isnull(ch.com_qu,0) as com_qu,sm.EditionHandle datel2 from 
	(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
		sum(cast(com_qu as decimal(10,0))) com_qu 
		from kucun where use_id=@uid  and com_qu>0 
		group by replace(sty_no,'','')+replace(col_no,'','')) kc
	left join
	(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
		sum(cast(com_qu as decimal(10,0))) com_qu 
		from chuhuosq('1',@dt1,@dt2) where Use_ent=@uid
		group by replace(sty_no,'','')+replace(col_no,'','')) ch
		on kc.styleno=ch.styleno
	--加入上市日期
	left join style_editionhandle sm
		on sm.StyleCode=kc.styleno
) l2 on l2.styleno=m.legging2
--下身1信息
left join
(
select kc.styleno,kc.com_qu+isnull(ch.com_qu,0) as com_qu,sm.EditionHandle dateb1 from 
	(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
		sum(cast(com_qu as decimal(10,0))) com_qu 
		from kucun where use_id=@uid  and com_qu>0 
		group by replace(sty_no,'','')+replace(col_no,'','')) kc
	left join
	(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
		sum(cast(com_qu as decimal(10,0))) com_qu 
		from chuhuosq('1',@dt1,@dt2) where Use_ent=@uid
		group by replace(sty_no,'','')+replace(col_no,'','')) ch
		on kc.styleno=ch.styleno
	--加入上市日期
	left join style_editionhandle sm
		on sm.StyleCode=kc.styleno
) b1 on b1.styleno=m.bottom1

--下身2信息
left join
(
select kc.styleno,kc.com_qu+isnull(ch.com_qu,0) as com_qu,sm.EditionHandle dateb2 from 
	(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
		sum(cast(com_qu as decimal(10,0))) com_qu 
		from kucun where use_id=@uid  and com_qu>0 
		group by replace(sty_no,'','')+replace(col_no,'','')) kc
	left join
	(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
		sum(cast(com_qu as decimal(10,0))) com_qu 
		from chuhuosq('1',@dt1,@dt2) where Use_ent=@uid
		group by replace(sty_no,'','')+replace(col_no,'','')) ch
		on kc.styleno=ch.styleno
	--加入上市日期
	left join style_editionhandle sm
		on sm.StyleCode=kc.styleno
) b2 on b2.styleno=m.bottom2
--配饰1信息
left join
(
	select kc.styleno,kc.com_qu+isnull(ch.com_qu,0) as com_qu,sm.EditionHandle datea1 from 
	(	 select replace(sty_no,'','')+replace(col_no,'','') styleno,
		 sum(cast(com_qu as decimal(10,0))) com_qu 
		 from kucun where use_id=@uid  and com_qu>0 
		 group by replace(sty_no,'','')+replace(col_no,'','')) kc
	 left join
	(	 select replace(sty_no,'','')+replace(col_no,'','') styleno,
		 sum(cast(com_qu as decimal(10,0))) com_qu 
		 from chuhuosq('1',@dt1,@dt2) where Use_ent=@uid
		 group by replace(sty_no,'','')+replace(col_no,'','')) ch
		 on kc.styleno=ch.styleno
	--加入上市日期
	left join style_editionhandle sm
		on sm.StyleCode=kc.styleno
) a1 on a1.styleno=m.accessory1
--配饰2信息
left join
(
select kc.styleno,kc.com_qu+isnull(ch.com_qu,0) as com_qu,sm.EditionHandle datea2 from 
	(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
		sum(cast(com_qu as decimal(10,0))) com_qu 
		from kucun where use_id=@uid  and com_qu>0 
		group by replace(sty_no,'','')+replace(col_no,'','')) kc
	left join
	(	select replace(sty_no,'','')+replace(col_no,'','') styleno,
		sum(cast(com_qu as decimal(10,0))) com_qu 
		from  chuhuosq('1',@dt1,@dt2) where Use_ent=@uid
		group by replace(sty_no,'','')+replace(col_no,'','')) ch
		on kc.styleno=ch.styleno
	--加入上市日期
	left join style_editionhandle sm
		on sm.StyleCode=kc.styleno
)a2 on a2.styleno=m.accessory2

----组合图信息
left join
(
	select cbpicture from tjdp where use_id=@uid and tjdate=@dt2 
) t
on t.cbpicture=m.cbpicture

--order by m.masterstyle,m.com_qu desc
go

--入库函数
CREATE  function [dbo].[ruku]
(
	 @sltType varchar(10),
	 @dt1 varchar(20), 
	 @dt2 varchar(20)
)
returns @ruku table
(
	inp_dt varchar(50),
	com_qu decimal(10,2),
	col_no varchar(30),
	sty_no varchar(30)
)
as
	begin
		--SELECT Dh,Use_out,Use_ent,Inp_dt,
		--convert(decimal(10,2),Unt_pr) as Unt_pr,
		--convert(decimal(10,2),Cos_pr) as Cos_pr,
		--convert(decimal(10,2),Com_pr) as Com_pr,
		--convert(decimal(10,2),Com_qu) as Com_qu,
		--out_nm,ent_nm,Com_id,Siz_dr,col_no,
		--Col_dr,Sty_no,Com_nm,Grp_id,Siz_id ,Sanc FROM 
		--(
		--	select *from ruku1 where
		--	Inp_dt>=@dt1 And Inp_dt<=@dt2
		--	union all
		--	select *from ruku2 where Inp_dt>=@dt1 And Inp_dt<=@dt2
		--) a
		if @sltType='1'
			begin
				insert into @ruku
				select * from 
				(
					select Inp_dt,convert(decimal(10,2),Com_qu) as Com_qu,
					col_no,Sty_no 
					from ruku1 where
					Inp_dt>=@dt1 And Inp_dt<=@dt2
					union all
					select Inp_dt,convert(decimal(10,2),Com_qu) as Com_qu,
					col_no,Sty_no
					from ruku2 where Inp_dt>=@dt1 And Inp_dt<=@dt2
				) a
			end
		else
			begin
				insert into @ruku
				select * from 
				(
					select Inp_dt,convert(decimal(10,2),Com_qu) as Com_qu,
					col_no,Sty_no 
					from ruku1 
					--where
					--cast(month(Inp_dt) as varchar(30))>=@dt1 
					--And cast( month(Inp_dt) as varchar(30))<=@dt2
					union all
					select Inp_dt,convert(decimal(10,2),Com_qu) as Com_qu,
					col_no,Sty_no
					from ruku2 
					--where 
					--cast(month(Inp_dt) as varchar(30))>=@dt1 
					--And cast( month(Inp_dt) as varchar(30))<=@dt2
				) a
			end
		return
	end
go

--退货函数
CREATE  function [dbo].[tuihuo]
(
	 @sltType varchar(10),
	 --@dpid varchar(20),
	 @dt1 varchar(20), 
	 @dt2 varchar(20)
	 --@dpids varchar(4000)
)
returns @tuihuo table
(
	out_nm varchar(50),
	use_out varchar(50),
	Inp_dt varchar(30),
	com_qu decimal(10,2),
	sty_no varchar(30),
	col_no varchar(30)
)
as
	begin
		--SELECT Dh,Qrd_no,Use_out,Use_ent,Inp_dt,Unt_pr,Cos_pr,Com_pr,
		--convert(decimal(10,2),Com_qu) as Com_qu,Com_nm,Sty_no,Com_id,Col_dr,
		--Siz_dr,Out_nm,col_no,Ent_nm,Grp_id,Siz_id ,Sanc FROM (Select Dh,
		--Qrd_no,Use_out,Use_ent,Inp_dt,Unt_pr,Cos_pr,Com_pr,Com_qu,Com_nm,
		--Dyan_no Sty_no,Com_id,Col_dr,Siz_dr,Out_nm,col_no,Ent_nm,Grp_id,Siz_id ,
		--'Y' Sanc from eissy.dbo.DyantoStandSty 
		--join eissy.dbo.vwC503 on eissy.dbo.vwC503.sty_no=eissy.dbo.DyantoStandSty.sty_no
		--Inner Join eissy.dbo.Fila11e On eissy.dbo.Fila11e.Use_id=eissy.dbo.vwC503.Use_Ent 
		--Inner Join eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=eissy.dbo.vwc503.Use_Out  
		--Where 1=1 And Inp_dt>=@dt1 And Inp_dt<=@dt2 And Com_id Like '%'
		--And fila11e.user_id='lfl' And FIla11e1.User_id='lfl' 
		--Union all 
		--Select Dh,Qrd_no,Use_out,Use_ent,Inp_dt,Unt_pr,Cos_pr,Com_pr,
		--convert(decimal(10,2),Com_qu) as Com_qu,Com_nm,Dyan_no Sty_no,
		--Com_id,Col_dr,Siz_dr,Out_nm,col_no,Ent_nm,Grp_id,Siz_id ,'N' Sanc 
		--from eissy.dbo.DyantoStandSty join eissy.dbo.vwCDraft503 on 
		--eissy.dbo.vwCDraft503.sty_no=eissy.dbo.DyantoStandSty.sty_no
		--Inner Join 
		--eissy.dbo.Fila11e On eissy.dbo.Fila11e.Use_id=eissy.dbo.vwCDraft503.Use_Ent 
		--Inner Join 
		--eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=eissy.dbo.vwCDraft503.Use_Out  
		--Where 1=1 And Inp_dt>=@dt1 And Inp_dt<=@dt2 And Com_id Like '%'
		--And eissy.dbo.fila11e.user_id='lfl' And FIla11e1.User_id='lfl'  ) a --ORDER BY Dh,Com_id
		--declare @tmp varchar(4000);
		--set @tmp=dbo.concatsql(@dpids);
		if @sltType='1'
			begin
				insert into @tuihuo
				select * from 
				(
					select Out_nm,Use_out,Inp_dt,
					convert(decimal(10,2),Com_qu) as Com_qu,Dyan_no Sty_no,col_no
					from eissy.dbo.DyantoStandSty 
					join eissy.dbo.vwC503 on eissy.dbo.vwC503.sty_no=eissy.dbo.DyantoStandSty.sty_no
					Inner Join eissy.dbo.Fila11e On eissy.dbo.Fila11e.Use_id=eissy.dbo.vwC503.Use_Ent 
					Inner Join eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=eissy.dbo.vwc503.Use_Out  
					Where 1=1 And Inp_dt>=@dt1 And Inp_dt<=@dt2 And Com_id Like '%'
					And fila11e.user_id='lfl' And FIla11e1.User_id='lfl' 
					--and use_out in (@tmp)
					Union all 
					Select Out_nm,Use_out,Inp_dt,
					convert(decimal(10,2),Com_qu) as Com_qu,Dyan_no Sty_no,col_no 
					from eissy.dbo.DyantoStandSty join eissy.dbo.vwCDraft503 on 
					eissy.dbo.vwCDraft503.sty_no=eissy.dbo.DyantoStandSty.sty_no
					Inner Join 
					eissy.dbo.Fila11e On eissy.dbo.Fila11e.Use_id=eissy.dbo.vwCDraft503.Use_Ent 
					Inner Join 
					eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=eissy.dbo.vwCDraft503.Use_Out  
					Where 1=1 And Inp_dt>=@dt1 And Inp_dt<=@dt2 And Com_id Like '%'
					And eissy.dbo.fila11e.user_id='lfl' And FIla11e1.User_id='lfl'
					--and use_out in (@tmp) 
				) a 
			end
		else
			begin
				insert into @tuihuo
				select * from 
				(
					select Out_nm,Use_out,Inp_dt,
					convert(decimal(10,2),Com_qu) as Com_qu,Dyan_no Sty_no,col_no
					from eissy.dbo.DyantoStandSty 
					join eissy.dbo.vwC503 on eissy.dbo.vwC503.sty_no=eissy.dbo.DyantoStandSty.sty_no
					Inner Join eissy.dbo.Fila11e On eissy.dbo.Fila11e.Use_id=eissy.dbo.vwC503.Use_Ent 
					Inner Join eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=eissy.dbo.vwc503.Use_Out  
					Where 1=1 
					And Com_id Like '%'
					And fila11e.user_id='lfl' And FIla11e1.User_id='lfl'
					--and use_out in (@tmp)
					Union all 
					Select Out_nm,Use_out,Inp_dt,
					convert(decimal(10,2),Com_qu) as Com_qu,Dyan_no Sty_no,col_no 
					from eissy.dbo.DyantoStandSty join eissy.dbo.vwCDraft503 on 
					eissy.dbo.vwCDraft503.sty_no=eissy.dbo.DyantoStandSty.sty_no
					Inner Join 
					eissy.dbo.Fila11e On eissy.dbo.Fila11e.Use_id=eissy.dbo.vwCDraft503.Use_Ent 
					Inner Join 
					eissy.dbo.Fila11e Fila11e1 On Fila11e1.Use_id=eissy.dbo.vwCDraft503.Use_Out  
					Where 1=1 
					And Com_id Like '%'
					And eissy.dbo.fila11e.user_id='lfl' And FIla11e1.User_id='lfl' 
					--and use_out in (@tmp)
				) a 
			end
		return
	end
go


 --拼接字符串函数
create function [dbo].[concatsql]
(
	@cond VARCHAR(1024)
)
returns varchar(1000)
as
   begin
		declare @tmp varchar(1000)
		--set @tmp='('
		set @tmp=''
		declare @next int  
		declare @ids varchar(20)
		set @ids=''
		set @next=1
		--while @next<=eissydp.dbo.splitlen(@cond,',')
		while @next<=eissy.dbo.splitlen(@cond,',')
			begin
				--set @ids=eissydp.dbo.mysplit(@cond,',',@next) 
				set @ids=eissy.dbo.mysplit(@cond,',',@next) 
				set @tmp=@tmp+''''+@ids+''''+','
				set @next=@next+1
			end
		set @tmp=substring(@tmp,0,len(@tmp))--+')'
		return @tmp
   end

 go


CREATE FUNCTION [dbo].[MySplit]
 (
	 @str VARCHAR(1024),--原始字符串
	 @split VARCHAR(20),--分割字符
	 @index INT--获取给定序号的字符串值
 ) 
 RETURNS VARCHAR(1024)
 AS
 BEGIN 
	 DECLARE @location INT;--@split在@str出现的位置
	 DECLARE @start INT;--开始位置
	 DECLARE @next INT;--获取数组中的那个字符
	 DECLARE @seed INT;--分给字符的长度
	 SET @str=LTRIM(RTRIM(@str));
	 SET @start=1;
	 SET @next=1;
	 SET @seed=LEN(@split);
	 SET @location=CHARINDEX(@split,@str,@start);
	 WHILE @location<>0 AND @index>@next
	   BEGIN
		SET @start=@location+@seed;
		SET @location=CHARINDEX(@split,@str,@start);
		SET @next=@next+1;
	   END
	 IF @location=0 SELECT @location =LEN(@str)+1;
	 RETURN SUBSTRING(@str,@start,@location-@start)
 END
go


CREATE FUNCTION [dbo].[splitLen](@str VARCHAR(1024),@split VARCHAR(2))
RETURNS INT
AS
   BEGIN
       DECLARE @location INT
       DECLARE @start INT 
       DECLARE @len INT
       set @str=LTRIM(RTRIM(@str))
       SET @location=CHARINDEX(@split,@str);
       SET @len=1;
       WHILE @location<>0
           BEGIN
              SET @start=@location+1;
              SET @location=charindex(@split,@str,@start)
              SET @len=@len+1
           END
              RETURN @len
   END
go



--触发器 （up4当中的，为了让上市日期更新到表style_editionhandle：专门用来记录上市日期而创建的）
create trigger tr_insert_style_editionhandle
on sm_style
for insert 
as

	declare @code varchar(50);
	declare @editionhandle varchar(50);
	select @code=code from inserted;
	select @editionhandle=editionhandle from inserted;
	delete from 
	opendatasource('sqloledb','data source=.;user id=sa;password=cosen').eissy.dbo.style_editionhandle
	where stylecode=@code
	insert into opendatasource('sqloledb','data source=.;user id=sa;password=cosen').eissy.dbo.style_editionhandle
	(StyleCode,EditionHandle) values(@code,@editionhandle)
go


create trigger tr_update_style_editionhandle
on sm_style
for update 
as
	declare @code varchar(50);
	declare @editionhandle varchar(50);
	select @code=code from inserted;
	select @editionhandle=editionhandle from inserted;
	delete from 
	opendatasource('sqloledb','data source=.;user id=sa;password=cosen').eissy.dbo.style_editionhandle
	where stylecode=@code
	insert into opendatasource('sqloledb','data source=.;user id=sa;password=cosen').eissy.dbo.style_editionhandle
	(StyleCode,EditionHandle) values(@code,@editionhandle)

go
