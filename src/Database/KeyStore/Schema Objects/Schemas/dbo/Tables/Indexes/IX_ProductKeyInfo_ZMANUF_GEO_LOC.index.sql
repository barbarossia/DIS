﻿CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZMANUF_GEO_LOC]
    ON [dbo].[ProductKeyInfo]([ZMANUF_GEO_LOC] ASC) WHERE ([ZMANUF_GEO_LOC] IS NOT NULL)
    ON [PRIMARY];

