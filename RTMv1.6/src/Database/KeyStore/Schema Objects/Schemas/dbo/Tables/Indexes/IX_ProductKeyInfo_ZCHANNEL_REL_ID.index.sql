﻿CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZCHANNEL_REL_ID]
    ON [dbo].[ProductKeyInfo]([ZCHANNEL_REL_ID] ASC) WHERE ([ZCHANNEL_REL_ID] IS NOT NULL)
    ON [PRIMARY];

