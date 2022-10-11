SELECT        NAME, DIP_LEVEL AS "Current Level", CALC_CLOSING_GRS_VOL_BBLS AS "Current volume", OPENING_GRS_VOL_BBLS AS "Previous volume"
FROM            ECKERNEL_DHA.RV_TANK_DAY_INV_OIL
WHERE        (OP_SUB_AREA_CODE = 'JALALABAD_OPS') AND (PRODUCTION_DAY = :PARAM2)
UNION
SELECT        NAME, MAX_VOLUME AS "Current Level", CLOSING AS "Current volume", OPENING AS "Previous volume"
FROM            ECKERNEL_DHA.RV_CHEM_TANK_STATUS
WHERE        (OP_SUB_AREA_CODE = 'JALALABAD_OPS') AND (PRODUCTION_DAY = :PARAM2) AND (CODE = 'JB_METHANOL_TANK_T201')