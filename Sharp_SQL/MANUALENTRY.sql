
select  TCR_TERMINAL,'MANUAL ENTRY',
case substr(TCR_OPTIONS,8,1) when '1' then 'MANUAL ENTRY' when '0' then 'NOT ALLOWED' else 'N/A' end as CVV,
TCR_OPTIONS from  TERM_CARD_TYPE_RANGE  where
TCR_TERMINAL in (select  terminal.TER_TID from terminal
where ter_cluster in ('308','315'))
and TCR_CARD_TYPE <> 999
group by tcr_terminal, TCR_OPTIONS

--BIN range level FLAGS
--#define CTO_TYPE       0         // Card type (0-bankcard, other-fuelcard)
--#define CTO_PRINT      1         // Disable slip printing (fuelcard only)
--#define CTO_LUHN       2         // LUHN check digit checking enabled
--#define CTO_EXP        3         // Expiry date checking enabled
--#define CTO_SERV       4         // Service Code checking enabled
--#define CTO_PIN        5         // PIN code required (0-no, 1-PIN bypass allowed, 2-mandatory)
--#define CTO_DOMESTIC   6         // Domestic checking enabled (fuelcard only)
--#define CTO_MANUAL     7         // Manual entry mode enabled
--#define CTO_RECEIPT    8         // Receipt type (RFU)
--#define CTO_FALLBACK   9         // Fallback disabled
--#define CTO_PAN_MASK   10        // PAN masking on ECR (RFU)
