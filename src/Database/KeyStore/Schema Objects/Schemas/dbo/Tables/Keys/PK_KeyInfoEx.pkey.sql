﻿ALTER TABLE [dbo].[KeyInfoEx]
    ADD CONSTRAINT [PK_KeyInfoEx] PRIMARY KEY CLUSTERED ([ProductKeyID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

