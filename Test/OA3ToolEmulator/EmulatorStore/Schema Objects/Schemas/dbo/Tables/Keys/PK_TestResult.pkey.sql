﻿ALTER TABLE [dbo].[TestResult]
    ADD CONSTRAINT [PK_TestResult] PRIMARY KEY CLUSTERED ([TestResultID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

