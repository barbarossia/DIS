﻿ALTER TABLE [dbo].[KeyInfoEx]
    ADD CONSTRAINT [FK_KeyInfoEx_HeadQuarter] FOREIGN KEY ([HQID]) REFERENCES [dbo].[HeadQuarter] ([HeadQuarterID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

