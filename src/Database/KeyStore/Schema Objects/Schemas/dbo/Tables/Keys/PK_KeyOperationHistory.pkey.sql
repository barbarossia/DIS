﻿ALTER TABLE [dbo].[KeyOperationHistory]
    ADD CONSTRAINT [PK_KeyOperationHistory] PRIMARY KEY CLUSTERED ([OperationID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

