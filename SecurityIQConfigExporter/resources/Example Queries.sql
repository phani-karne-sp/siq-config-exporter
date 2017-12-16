--SIQ 5.1 queries for exporter tool
--2017 12 15 (Fri)

--dbInfo1
--select @@SERVERNAME [DB ServerName], @@VERSION Version;

--dbinfo2
--SELECT [create_date] as [Create Date], [compatibility_level] as [Compatability Level], is_broker_enabled as [SQL Broker Enabled], collation_name as [DB Collation], recovery_model_desc as [Recovery Model] FROM sys.databases WHERE name = 'SecurityIQDB';

--dbinfo3
--SELECT 
--      database_name = DB_NAME(database_id)
--    , log_size_mb = CAST(SUM(CASE WHEN type_desc = 'LOG' THEN size END) * 8. / 1024 AS DECIMAL(8,2))
--    , row_size_mb = CAST(SUM(CASE WHEN type_desc = 'ROWS' THEN size END) * 8. / 1024 AS DECIMAL(8,2))
--    , total_size_mb = CAST(SUM(size) * 8. / 1024 AS DECIMAL(8,2))
--FROM sys.master_files WITH(NOWAIT)
--WHERE database_id = DB_ID()
--GROUP BY database_id

--dbinfo4
--https://stackoverflow.com/questions/7892334/get-size-of-all-tables-in-database
--SELECT top 25
--    t.NAME AS [Table Name],
--    s.Name AS [Schema Name],
--    p.rows AS [Row Counts],
--    CAST(ROUND(((SUM(a.total_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS [Total Space MB], 
--    CAST(ROUND(((SUM(a.used_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS [Used Space MB], 
--    CAST(ROUND(((SUM(a.total_pages) - SUM(a.used_pages)) * 8) / 1024.00, 2) AS NUMERIC(36, 2)) AS [Unused Space MB]
--FROM 
--    sys.tables t
--INNER JOIN      
--    sys.indexes i ON t.OBJECT_ID = i.object_id
--INNER JOIN 
--    sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
--INNER JOIN 
--    sys.allocation_units a ON p.partition_id = a.container_id
--LEFT OUTER JOIN 
--    sys.schemas s ON t.schema_id = s.schema_id
--WHERE 
--    t.NAME NOT LIKE 'dt%' 
--    AND t.is_ms_shipped = 0
--    AND i.OBJECT_ID > 255
--GROUP BY 
--    t.Name, s.Name, p.Rows
--ORDER BY 
--    [Total Space MB] desc




--siqConfig1
--select scv.name as [Config Item], scv.value as [Value] from [whiteops].[system_configuration_value] scv where scv.name in('Company Name','Version','Elastic Search data path', 'Report Web Access Site URL', 'WBX Business Site') order by scv.name;

--siqConfig2
--select name as [Config Item], value as [Value] from [crowdSource].[system_configuration_value] where name in('SMTP_HOST', 'SMTP_PORT','Web Site URL','SystemEmailAddress') order by name;


--license
--select l.module_name as [Module Name], l.sku as [SKU], l.description as [Description], l.license_utilization as [Utilization], l.expiration_date as [Expiration Date] from [whiteops].[license] l order by [Module Name];


--installServer
--select name as [Name], home_path as [Home Path], log_path as [Log Path] from [whiteops].[install_server] order by id;

--installService
--select ins.name as [Service Name], ins.technical_name as [Technical Name], inss.name as [Associated Server], 
--case when ins.port = -1 then null else ins.port end as [Port], case when ins.default_port = -1 then null else ins.default_port end as [Default Port], 
--insc.name as [Service Type], ins.status_update_date as [Status Update Date], ins.agent_name as [Agent Name], ins.version as [Version] 
--from whiteops.install_service ins, whiteops.install_service_category insc, whiteops.install_server inss 
--where ins.install_service_category_id = insc.id 
--	and ins.installed_server_id = inss.id 
--order by inss.name;

--idc (id collector)
--select name as [IDC Name], is_authentication_store as [Is Authentication Store], access_fulfillment_enabled as [Access Fulfillment Enabled] from whiteops.ra_identity_collector where id > 0;

--wpc (DEC)
--select wpc.name [DEC Name], wpct.name [DEC Type], wpc.wpc_configuration_id [WPC Configuration Id], wpc.is_logical_wpc [Is Logical WPC], wpc.status_update_date [Status Update Date], 
--wpc.logical_status_message [Status Message], wpc.first_seen_alive [First Seen] 
--from whiteops.wpc wpc, whiteops.wpc_type wpct 
--where wpc.id > 999 and wpc.wpc_type_id = wpct.id;


--apps
--select c.name [App Name], b.name [BAM Name], b.[description] [Description] , b.domain_netbios_name [Domain NetBIOS Name], b.first_crawled_date [First Crawl], b.last_crawling_time [Last Crawl], 
--bt.name [BAM Type], bt.group_code_name [BAM Group], bt.supports_data_classification [Supports Data Classification], b.data_classification_installed [DC Installed], raidc.name [Identity Collector Name], 
--pb.installed [BAM Installed], pb.logical_status_message [Status Message], pb.host_address [Host Name], isrv.name [Event Manager Name], isrv2.agent_name [Permissions Collector] 
--from 
--	whiteops.bam b, 
--	whiteops.bam_type bt, 
--	whiteops.ra_identity_collector raidc,
--	whiteops.physical_bam pb, 
--	whiteops.install_service isrv, 
--	whiteops.install_service isrv2, 
--	whiteops.container c
--where
--	b.bam_type_id = bt.id 
--	and b.ra_identity_collector_id = raidc.id 
--	and b.bam_configuration_id = pb.bam_id 
--	and b.event_manager_service_id = isrv.id 
--	and b.ra_service_id = isrv2.id 
--	and b.container_id = c.id;
	

--appConfigs
--select c.name [Container],  b.name [Name], bcv.name [Config Item], bcv.value [Value] 
--from whiteops.bam_configuration_value bcv, whiteops.bam b, whiteops.container c 
--where b.bam_configuration_id = bcv.bam_configuration_id 
--and b.container_id = c.id 
--and bcv.name NOT IN('password','pkcs7Password') 
--order by c.name, b.name, bcv.name;

--tasks
--select st.[name] [Task Name], tt.name [Task Type], tst.name [Task SubType], st.created_by [Created By], next_run [Next Run], once_date [Once Date], schedt.name [Frequency] 
--from whiteops.schedule_task st, whiteops.task_type tt,whiteops.task_sub_type tst, whiteops.schedule_type schedt 
--where st.task_type_id = tt.id 
--and st.task_sub_type_id = tst.id 
--and st.schedule_type_id = schedt.id;

--taskResults
--select t.[name] [Task Name], tt.name [Task Type], tst.name [Task SubType], schedt.name [Frequency], t.start_date [Start Date], t.end_date [End Date], 
--t.progress [Progress], t.result [Result], t.details [Details], [is].agent_name [Agent Name] 
--from whiteops.task t 
--left join whiteops.task_type tt ON  t.task_type_id = tt.id 
--left join whiteops.task_sub_type tst ON t.task_sub_type_id = tst.id 
--left join whiteops.schedule_type schedt ON t.schedule_task_id = schedt.id 
--left join whiteops.install_service [is] ON t.install_service_id =  [is].id 
--order by t.name, t.start_date desc;

--healthCenter
--SELECT x.* 
--FROM ( SELECT DISTINCT name FROM whiteops.install_service ) c 
--CROSS APPLY ( 
--	SELECT TOP 100 hce.date [Date], ts.name [Task Status], ins.agent_name [Agent Name], ins.name [Service Name], hce.title [Title], hce.details [Details], hce.level_enum_id [Level Enum Id]
--	from [whiteops].[health_center_event]  hce, whiteops.install_service ins, whiteops.task_status ts 
--	where ins.id = hce.source_id and hce.task_status_enum_id = ts.id and c.name=ins.name order by source_id, [date] desc) x 
--order by [Service Name], [Date] desc;


--users
--select u.name [User Name], u.first_name + ' ' + u.last_name [Full Name], u.last_login [Last SIQ Login], ra.user_full_name [Connected User] 
--from whiteops.[user] u 
--left join whiteops.ra_user ra on u.ra_user_id = ra.id
--order by [User Name];

