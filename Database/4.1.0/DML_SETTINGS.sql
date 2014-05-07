/* Use this script to add the V-MANAGER application configuration settings       */

PRINT 'ADDING APPSETTING RECORDS FOR V-MANAGER.'

PRINT '';
USE [DOM_Config]
GO

BEGIN TRANSACTION
GO
ALTER TABLE vmg.[Log] ADD
	CorrelationId nvarchar(100) NULL
GO
ALTER TABLE vmg.[Log] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
GO

PRINT @@SERVERNAME

DECLARE 
	  @SqlInstanceName varchar(max)
	, @Environment varchar(32) = 'DOMSPW327D';
	
IF @@SERVERNAME LIKE 'DOMSP002'	
	BEGIN
		SET @Environment = 'LOCAL';
		SET @SqlInstanceName = 'DOMSPW327D';
	END
	
IF @@SERVERNAME LIKE 'W2K3R2X86SP1\SQL1'	
	BEGIN
		SET @Environment = 'DEV3';
		SET @SqlInstanceName = 'DEV3\SQL1';
	END
	
IF @@SERVERNAME LIKE 'INBSPSQLW801T\DINTCONFIG1'	
	BEGIN
		SET @Environment = 'DEV';
		SET @SqlInstanceName = 'INBSPSQLW801T\DINTCONFIG1';
	END
	
	
IF @@SERVERNAME LIKE 'INBSPSQLW812T\INTCONFIG1'	
	BEGIN
		SET @Environment = 'TEST';
		SET @SqlInstanceName = 'INBSQLW820D';
	END
	
IF @@SERVERNAME like 'OJRSPSQLW811Q\INTCONFIG1'
	BEGIN
		SET @Environment = 'QA';
		SET @SqlInstanceName = 'INBSQLW820Q';
	END
	
IF @@SERVERNAME like 'OJRSPSSQL1\INTCONFIG1'
	BEGIN
		SET @Environment = 'PROD';
		SET @SqlInstanceName = 'OJRSQLW820';
	END
	
PRINT '@Environment = ' + @Environment;
PRINT '@ServerName = ' + @SqlInstanceName;
PRINT '';

DELETE FROM vmg.Setting WHERE [Key] = N'VmgrLogger'

IF NOT EXISTS (SELECT * FROM vmg.Setting WHERE [Key] = N'VmgrLogger')
	BEGIN
		IF @Environment = 'LOCAL'
			BEGIN
				PRINT 'ADDING THE [VmgrLogger] SETTING TO [' + @Environment + '].';
				INSERT INTO vmg.Setting
				(
					[Key]  
					,[Value] 
					,[Cache] 
					,[CreateUser]
				)
				VALUES
				(
					'VmgrLogger'
					,N'
					<log4net>
						<logger name="VmgrLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrAppender" />
						</logger>
						<logger name="VmgrSmtpLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrSmtpAppender" />
						</logger>
						<appender name="VmgrSmtpAppender" type="log4net.Appender.SmtpAppender">
						  <to value="todd.s.worden@com.com" />
						  <from value="v-manager@dom.com" />
						  <subject value="Error in V-Manager System - DEV" />
						  <smtpHost value="smtpmta.va.dominionnet.com" />
						  <bufferSize value="256" />
						  <lossy value="true" />
						  <evaluator type="log4net.Core.LevelEvaluator">
							<threshold value="ERROR" />
						  </evaluator>
						  <layout type="log4net.Layout.PatternLayout">
							<conversionPattern value="Date: %date{yyyy/MM/dd HH:mm:ss}.%newlineLevel: %level%newlineMessage: %message%newlineStack Trace: %stacktracedetail%newline%newline" />
						  </layout>
						</appender>
						<appender name="VmgrAppender" type="log4net.Appender.ADONetAppender">
							<bufferSize value="1" />
							<!--Record is written immediately if set to 1-->
							<connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
							<connectionString value="server=DOMSPW327D; initial Catalog=DOM_Config; integrated security=SSPI;persist security info=False;packet size=4096;" />
							<commandText value="INSERT INTO [vmg].[Log] ( [Thread], [ThreadId], [ApplicationName], [Level], [Logger], [Message], [Exception], [CreateDate], [CreateUser], [Server], [CorrelationId] ) VALUES ( @thread, @threadId,@applicationName,@log_level, @logger, @message, @exception, @log_date, @log_user, @server, @correlationId)" />
							<parameter>
								<parameterName value="@thread" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%thread" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@threadId" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{threadId}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@applicationName" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{applicationName}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@log_level" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%level" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@logger" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%logger" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@message" />
								<dbType value="String" />
								<size value="4000" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%message" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@exception" />
								<dbType value="String" />
								<size value="2000" />
								<layout type="log4net.Layout.ExceptionLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_date" />
								<dbType value="DateTime" />
								<layout type="log4net.Layout.RawTimeStampLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_user" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{user}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@server" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{server}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@correlationId" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{correlationId}" />
								</layout>
							</parameter>
						</appender>
					</log4net>
					'
					,1
					,'System'
				)
			END
		IF @Environment = 'DEV3'
			BEGIN
				PRINT 'ADDING THE [VmgrLogger] SETTING TO [' + @Environment + '].';
				INSERT INTO vmg.Setting
				(
					[Key]  
					,[Value] 
					,[Cache] 
					,[CreateDate]
					,[CreateUser]
				)
				VALUES
				(
					'VmgrLogger'
					,N'
					<log4net>
						<logger name="VmgrLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrAppender" />
						</logger>
						<logger name="VmgrSmtpLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrSmtpAppender" />
						</logger>
						<appender name="VmgrSmtpAppender" type="log4net.Appender.SmtpAppender">
						  <to value="todd@web-wired.com" />
						  <from value="v-manager@dom.com" />
						  <subject value="Error in V-Manager System - DEV2" />
						  <smtpHost value="mail.web-wired.com" />
						  <bufferSize value="256" />
						  <lossy value="true" />
						  <evaluator type="log4net.Core.LevelEvaluator">
							<threshold value="ERROR" />
						  </evaluator>
						  <layout type="log4net.Layout.PatternLayout">
							<conversionPattern value="Date: %date{yyyy/MM/dd HH:mm:ss}.%newlineLevel: %level%newlineMessage: %message%newlineStack Trace: %stacktracedetail%newline%newline" />
						  </layout>
						</appender>
						<appender name="VmgrAppender" type="log4net.Appender.ADONetAppender">
							<bufferSize value="1" />
							<!--Record is written immediately if set to 1-->
							<connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
							<connectionString value="server=DEV3\SQL1; initial Catalog=DOM_Config; integrated security=SSPI;persist security info=False;packet size=4096;" />
							<commandText value="INSERT INTO [vmg].[Log] ( [Thread], [ThreadId], [ApplicationName], [Level], [Logger], [Message], [Exception], [CreateDate], [CreateUser], [Server], [CorrelationId] ) VALUES ( @thread, @threadId,@applicationName,@log_level, @logger, @message, @exception, @log_date, @log_user, @server, @correlationId)" />
							<parameter>
								<parameterName value="@thread" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%thread" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@threadId" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{threadId}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@applicationName" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{applicationName}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@log_level" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%level" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@logger" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%logger" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@message" />
								<dbType value="String" />
								<size value="4000" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%message" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@exception" />
								<dbType value="String" />
								<size value="2000" />
								<layout type="log4net.Layout.ExceptionLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_date" />
								<dbType value="DateTime" />
								<layout type="log4net.Layout.RawTimeStampLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_user" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{user}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@server" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{server}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@correlationId" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{correlationId}" />
								</layout>
							</parameter>
						</appender>
					</log4net>
					'
					,1
					,GETDATE()
					,'System'
				)
			END
		IF @Environment = 'DEV'
			BEGIN
				PRINT 'ADDING THE [VmgrLogger] SETTING TO [' + @Environment + '].';
				INSERT INTO vmg.Setting
				(
					[Key]  
					,[Value] 
					,[Cache] 
					,[CreateUser]
				)
				VALUES
				(
					'VmgrLogger'
					,N'
					<log4net>
						<logger name="VmgrLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrAppender" />
						</logger>
						<logger name="VmgrSmtpLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrSmtpAppender" />
						</logger>
						<appender name="VmgrSmtpAppender" type="log4net.Appender.SmtpAppender">
						  <to value="todd.s.worden@dom.com" />
						  <from value="v-manager@dom.com" />
						  <subject value="Error in V-Manager System - TEST" />
						  <smtpHost value="smtpmta.va.dominionnet.com" />
						  <bufferSize value="256" />
						  <lossy value="true" />
						  <evaluator type="log4net.Core.LevelEvaluator">
							<threshold value="ERROR" />
						  </evaluator>
						  <layout type="log4net.Layout.PatternLayout">
							<conversionPattern value="Date: %date{yyyy/MM/dd HH:mm:ss}.%newlineLevel: %level%newlineMessage: %message%newlineStack Trace: %stacktracedetail%newline%newline" />
						  </layout>
						</appender>
						<appender name="VmgrAppender" type="log4net.Appender.ADONetAppender">
							<bufferSize value="1" />
							<!--Record is written immediately if set to 1-->
							<connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
							<connectionString value="server=DOMSPW327D; initial Catalog=DOM_Config; integrated security=SSPI;persist security info=False;packet size=4096;" />
							<commandText value="INSERT INTO [vmg].[Log] ( [Thread], [ThreadId], [ApplicationName], [Level], [Logger], [Message], [Exception], [CreateDate], [CreateUser], [Server], [CorrelationId] ) VALUES ( @thread, @threadId,@applicationName,@log_level, @logger, @message, @exception, @log_date, @log_user, @server, @correlationId)" />
							<parameter>
								<parameterName value="@thread" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%thread" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@threadId" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{threadId}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@applicationName" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{applicationName}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@log_level" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%level" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@logger" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%logger" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@message" />
								<dbType value="String" />
								<size value="4000" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%message" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@exception" />
								<dbType value="String" />
								<size value="2000" />
								<layout type="log4net.Layout.ExceptionLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_date" />
								<dbType value="DateTime" />
								<layout type="log4net.Layout.RawTimeStampLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_user" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{user}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@server" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{server}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@correlationId" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{correlationId}" />
								</layout>
							</parameter>
						</appender>
					</log4net>
					'
					,1
					,'System'
				)
			END
		IF @Environment = 'TEST'
			BEGIN
				PRINT 'ADDING THE [VmgrLogger] SETTING TO [' + @Environment + '].';
				INSERT INTO vmg.Setting
				(
					[Key]  
					,[Value] 
					,[Cache] 
					,[CreateUser]
				)
				VALUES
				(
					'VmgrLogger'
					,N'
					<log4net>
						<logger name="VmgrLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrAppender" />
						</logger>
						<logger name="VmgrSmtpLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrSmtpAppender" />
						</logger>
						<appender name="VmgrSmtpAppender" type="log4net.Appender.SmtpAppender">
						  <to value="todd.s.worden@dom.com" />
						  <from value="v-manager@dom.com" />
						  <subject value="Error in V-Manager System - TEST" />
						  <smtpHost value="smtpmta.va.dominionnet.com" />
						  <bufferSize value="256" />
						  <lossy value="true" />
						  <evaluator type="log4net.Core.LevelEvaluator">
							<threshold value="ERROR" />
						  </evaluator>
						  <layout type="log4net.Layout.PatternLayout">
							<conversionPattern value="Date: %date{yyyy/MM/dd HH:mm:ss}.%newlineLevel: %level%newlineMessage: %message%newlineStack Trace: %stacktracedetail%newline%newline" />
						  </layout>
						</appender>
						<appender name="VmgrAppender" type="log4net.Appender.ADONetAppender">
							<bufferSize value="1" />
							<!--Record is written immediately if set to 1-->
							<connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
							<connectionString value="server=INBSPSQLW812T\intconfig1; initial Catalog=DOM_Config; integrated security=SSPI;persist security info=False;packet size=4096;" />
							<commandText value="INSERT INTO [vmg].[Log] ( [Thread], [ThreadId], [ApplicationName], [Level], [Logger], [Message], [Exception], [CreateDate], [CreateUser], [Server], [CorrelationId] ) VALUES ( @thread, @threadId,@applicationName,@log_level, @logger, @message, @exception, @log_date, @log_user, @server, @correlationId)" />
							<parameter>
								<parameterName value="@thread" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%thread" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@threadId" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{threadId}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@applicationName" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{applicationName}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@log_level" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%level" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@logger" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%logger" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@message" />
								<dbType value="String" />
								<size value="4000" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%message" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@exception" />
								<dbType value="String" />
								<size value="2000" />
								<layout type="log4net.Layout.ExceptionLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_date" />
								<dbType value="DateTime" />
								<layout type="log4net.Layout.RawTimeStampLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_user" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{user}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@server" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{server}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@correlationId" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{correlationId}" />
								</layout>
							</parameter>
						</appender>
					</log4net>
					'
					,1
					,'System'
				)
			END
		IF @Environment = 'QA'
			BEGIN
				PRINT 'ADDING THE [VmgrLogger] SETTING TO [' + @Environment + '].';
				INSERT INTO vmg.Setting
				(
					[Key]  
					,[Value] 
					,[Cache] 
					,[CreateUser]
				)
				VALUES
				(
					'VmgrLogger'
					,N'
					<log4net>
						<logger name="VmgrLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrAppender" />
						</logger>
						<logger name="VmgrSmtpLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrSmtpAppender" />
						</logger>
						<appender name="VmgrSmtpAppender" type="log4net.Appender.SmtpAppender">
						  <to value="App_VMGSptTeam_6@dom.com" />
						  <from value="v-manager@dom.com" />
						  <subject value="Error in V-Manager System - QA" />
						  <smtpHost value="smtpmta.va.dominionnet.com" />
						  <bufferSize value="256" />
						  <lossy value="true" />
						  <evaluator type="log4net.Core.LevelEvaluator">
							<threshold value="ERROR" />
						  </evaluator>
						  <layout type="log4net.Layout.PatternLayout">
							<conversionPattern value="Date: %date{yyyy/MM/dd HH:mm:ss}.%newlineLevel: %level%newlineMessage: %message%newlineStack Trace: %stacktracedetail%newline%newline" />
						  </layout>
						</appender>
						<appender name="VmgrAppender" type="log4net.Appender.ADONetAppender">
							<bufferSize value="1" />
							<!--Record is written immediately if set to 1-->
							<connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
							<connectionString value="server=OJRSPSQLW811Q\INTCONFIG1; initial Catalog=DOM_Config; integrated security=SSPI;persist security info=False;packet size=4096;" />
							<commandText value="INSERT INTO [vmg].[Log] ( [Thread], [ThreadId], [ApplicationName], [Level], [Logger], [Message], [Exception], [CreateDate], [CreateUser], [Server], [CorrelationId] ) VALUES ( @thread, @threadId,@applicationName,@log_level, @logger, @message, @exception, @log_date, @log_user, @server, @correlationId)" />
							<parameter>
								<parameterName value="@thread" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%thread" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@threadId" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{threadId}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@applicationName" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{applicationName}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@log_level" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%level" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@logger" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%logger" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@message" />
								<dbType value="String" />
								<size value="4000" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%message" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@exception" />
								<dbType value="String" />
								<size value="2000" />
								<layout type="log4net.Layout.ExceptionLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_date" />
								<dbType value="DateTime" />
								<layout type="log4net.Layout.RawTimeStampLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_user" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{user}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@server" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{server}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@correlationId" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{correlationId}" />
								</layout>
							</parameter>
						</appender>
					</log4net>
					'
					,1
					,'System'
				)
			END		
		IF @Environment = 'PROD'
			BEGIN
				PRINT 'ADDING THE [VmgrLogger] SETTING TO [' + @Environment + '].';
				INSERT INTO vmg.Setting
				(
					[Key]  
					,[Value] 
					,[Cache] 
					,[CreateUser]
				)
				VALUES
				(
					'VmgrLogger'
					,N'
					<log4net>
						<logger name="VmgrLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrAppender" />
						</logger>
						<logger name="VmgrSmtpLogger">
							<level value="ALL" />
							<appender-ref ref="VmgrSmtpAppender" />
						</logger>
						<appender name="VmgrSmtpAppender" type="log4net.Appender.SmtpAppender">
						  <to value="App_VMGSptTeam_6@dom.com" />
						  <from value="v-manager@dom.com" />
						  <subject value="Error in V-Manager System - PROD" />
						  <smtpHost value="smtpmta.va.dominionnet.com" />
						  <bufferSize value="256" />
						  <lossy value="true" />
						  <evaluator type="log4net.Core.LevelEvaluator">
							<threshold value="ERROR" />
						  </evaluator>
						  <layout type="log4net.Layout.PatternLayout">
							<conversionPattern value="Date: %date{yyyy/MM/dd HH:mm:ss}.%newlineLevel: %level%newlineMessage: %message%newlineStack Trace: %stacktracedetail%newline%newline" />
						  </layout>
						</appender>
						<appender name="VmgrAppender" type="log4net.Appender.ADONetAppender">
							<bufferSize value="1" />
							<!--Record is written immediately if set to 1-->
							<connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
							<connectionString value="server=OJRSPSSQL1\INTCONFIG1; initial Catalog=DOM_Config; integrated security=SSPI;persist security info=False;packet size=4096;" />
							<commandText value="INSERT INTO [vmg].[Log] ( [Thread], [ThreadId], [ApplicationName], [Level], [Logger], [Message], [Exception], [CreateDate], [CreateUser], [Server], [CorrelationId] ) VALUES ( @thread, @threadId,@applicationName,@log_level, @logger, @message, @exception, @log_date, @log_user, @server, @correlationId)" />
							<parameter>
								<parameterName value="@thread" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%thread" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@threadId" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{threadId}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@applicationName" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{applicationName}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@log_level" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%level" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@logger" />
								<dbType value="String" />
								<size value="255" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%logger" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@message" />
								<dbType value="String" />
								<size value="4000" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%message" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@exception" />
								<dbType value="String" />
								<size value="2000" />
								<layout type="log4net.Layout.ExceptionLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_date" />
								<dbType value="DateTime" />
								<layout type="log4net.Layout.RawTimeStampLayout" />
							</parameter>
							<parameter>
								<parameterName value="@log_user" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{user}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@server" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{server}" />
								</layout>
							</parameter>
							<parameter>
								<parameterName value="@correlationId" />
								<dbType value="String" />
								<size value="50" />
								<layout type="log4net.Layout.PatternLayout">
									<conversionPattern value="%X{correlationId}" />
								</layout>
							</parameter>
						</appender>
					</log4net>
					'
					,1
					,'System'
				)
			END		
		END		
