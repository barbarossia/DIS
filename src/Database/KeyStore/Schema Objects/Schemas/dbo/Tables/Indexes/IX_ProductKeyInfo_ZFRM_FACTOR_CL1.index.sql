﻿CREATE NONCLUSTERED INDEX [IX_ProductKeyInfo_ZFRM_FACTOR_CL1] ON [dbo].[ProductKeyInfo] 
(
    [ZFRM_FACTOR_CL1] ASC
)
WHERE ([ZFRM_FACTOR_CL1] IS NOT NULL)
    ON [PRIMARY];

