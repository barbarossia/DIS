﻿ALTER TABLE [dbo].[CategoryLog]
    ADD CONSTRAINT [FK_CategoryLog_Log] FOREIGN KEY ([LogID]) REFERENCES [dbo].[Log] ([LogID]) ON DELETE CASCADE ON UPDATE NO ACTION;


