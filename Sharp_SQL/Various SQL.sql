select terminal.ter_tid, TERMINAL.TER_MCC_TYPE,TERM_CLUSTER.TCL_NAME  from terminal
join  TERM_CLUSTER ON TERM_CLUSTER.TCL_ID = TERMINAL.TER_CLUSTER
where terminal.ter_tid in ('01445710','01445720','01446070','01446380','01446690','01446710','01447310','01447660','01447710','01447882','01447960','01448170','01448190','01448220','01448260','01448320','01448361','01448391','01448431','01448440','01448591','01448802','01448871','01448941','01448961','01449001','01449110','01449292','01449301','01449310','01449371','01449591','01449820','01449851','01449890','01450102','01450262','01450591','01450711','01450752','01450801','01450811','01450930','01453112','01453780','01453791','01456031','01458631','01460501','01470651','01473332','01479681','01481784','01486331','01486341','01487231','01487241','01490201','01491441','01497641','01499131','01502051','01504824','01515450','01515551','01515552','01516291','01516361','01516891','01517941','01518653','01518801','01518871','01518941','01519051','01519120','01519361','01519401','01519641','01519671','01520281','01520361','01520650','01520710','01520731','01521011','01521101','01521111','01521151','01521161','01521171','01521211','01521471','01521711','01521761','01522611','01522701','01522881','01522921','01522941','01523011','01523131','01523171','01523181','01523353','01523621','01523641','01523701','01523841','01523881','01523981','01524101','01524141','01524212','01524291','01524431','01524491','01524511','01524591','01524731','01524791','01524974','01525060','01525071','01525421','01525431','01525461','01525501','01525511','01525571','01525891','01525961','01526292','01526481','01526521','01526841','01526971','01527071','01527121','01527640','01527911','01527941','01528171','01528181','01528311','01528371','01528481','01528531','01528591','01528611','01528621','01528771','01528810','01528851','01528861','01528921','01529361','01529501','01529521','01529531','01529651','01529671','01529710','01529892','01529921','01529951','01529991','01530111','01530171','01530251','01530341','01530392','01530461','01530551','01531011','01531021','01531041','01531321','01531522','01531635','01531911','01531970','01532241','01532551','01532651','01533000','01533061','01533601','01533771','01533850','01534011','01534161','01534436','01534721','01534951','01535170','01535251','01535271','01535431','01535471','01535481','01535651','01535711','01536061','01536541','01536772','01536881','01537011','01537110','01537231','01537261','01537381','01537531','01537851','01538151','01538161','01538351','01538521','01538691','01538762','01538781','01538912','01539321','01539771','01539791','01539961','01540411','01540420','01540532','01540761','01540970','01541001','01541131','01541141','01541161','01541511','01541811','01541901','01541981','01542191','01542201','01542271','01542301','01542322','01542391','01542421','01542691','01543011','01543121','01543211','01543331','01543341','01543370','01543642','01543811','01544811','01546542','01547182','01550511','01557721','01569191','01569281','01569811','01569941','01570611','01570621','01571481','01571491','01571591','01571762','01571821','01572181','01572260','01572331','01572371','01572511','01572531','01572811','01572831','01573291','01573362','01573410','01573481','01573501','01573751','01573801','01573821','01574541','01574641','01574711','01574831','01574861','01574941','01574951','01574980','01575100','01575122','01575221','01575291','01575461','01575570','01575901','01575971','01575991','01576271','01576302','01576541','01576561','01576591','01576671','01576691','01576911','01576971','01577081','01577150','01577181','01577361','01577542','01577571','01577721','01577741','01577751','01577791','01577811','01577981','01578081','01578301','01578511','01578571','01578591','01579311','01579501','01579881','01580062','01580261','01580271','01580291','01580471','01581211','01581331','01581591','01582002','01582081','01582433','01582501','01582741','01583081','01583791','01584191','01584292','01584921','01591842','01597403','01599591','01604971','01604983','01622101','01622222','01636011','01646561','01648921','01679501','01684511','01686861','01695850','01724261','01741191','01742281','01749121','01754761','01757361','01761331','01761390','01762271','01772111','01772161','01772861','01773980','01773990','01774901','01775123','01775131','01786210','01786483','01786761','01790501','01795171','01796001','01797821','01805521','01808151','01814911','01815000','01815241','01815842','01816301','01817831','01817841','01818721','01818941','01822491','01822501','01822651','01823302','01823311','01824041','01824051','01826380','01826391','01826851','01826860','01827701','01828141','01828721','01828782','01828991','01830334','01830335','01830662','01831471','01832101','01832551','01833911','01835201','01837141','01837150','01838381','01838931','01839281','01839341','01841191','01841772','01841901','01841911','01841961','01841971','01842720','01842861','01842991','01843650','01843951','01844421','01845542','01846171','01848701','01849141','01850232','01853651','01853671','01854441','01855921','01855931','01860481','01860891','01860920','01860931','01861091','01862531','01862560','01862571','01862582','01862592','01864001','01866211','01866294','01867321','01867881','01868831','01871161','01874872','01874901','01874961','01877332','01878172','01878612','01881421','01882322','01884832','01886181','01899721','01919042','01981911','01056441','01058731','01062801','01063670','01064280','01064822','01065064','01067953','01068311','01069101','01069411','01069951','02070225','01073311','01073363','01073871','01074371','01074861','01074921','01075232','01075301','01075940','01076351','01077291','01077361','01077461','01077562','01077711','01078110','01078311','01078411','01078471','01078551','01078621','01078681','01079753','01080293','01080581','01081091','01081541','01081551','01082411','01082674','01082922','01083062','01083192','01083421','01084173','01084341','01084701','01087421','01087571','01087881','01087977','01088761','01089453','01090802','01090901','01091382','01091551','01091861','01092100','01092291','01092302','01092331','01092451','01093081')
and terminal.ter_mcc_type <> 'RESTAURANT' and TERM_CLUSTER.TCL_NAME <> 'PIRA ICT RESTAURANT'
-- and terminal.ter_mcc_type = 'RESTAURANT' and TERM_CLUSTER.TCL_NAME <> 'PIRA ICT RESTAURANT'
-- and terminal.ter_mcc_type <> 'RESTAURANT' and TERM_CLUSTER.TCL_NAME = 'PIRA ICT RESTAURANT'


('01007265','01027610','01064961','01092911','01098282','01102791','01105310','01110010','01110701','01112351','01115410','01115420','01115421','01115430','01115440','01115450','01115460','01115461','01115471','01115480','01115481','01115491','01117543','01117710','01118313','01126461','01129881','01130340','01130352','01137580','01143642','01149440','01151610','01154092','01160160','01164740','01165360','01169531','01170070','01172540','01179750','01179780','01182012','01185740','01185940','01187010','01187171','01191442','01192010','01198243','01199350','01200400','01200570','01204120','01204830','01205680','01209332','01233152','01234892','01260561','01268954','01269491','01291691','01302531','01306460','01306461','01306470','01306972','01307981','01331832','01344801','01345160','01345281','01345444','01346233','01346703','01349642','01350780','01360971','01377861','01380203','01387011','01390801','01406202','01412192','01422793','01436282','01437641','01492412','01501232','01580492','01644141','01677621','01690961','01713792','01791992','01841572','01844441','01861962','01918580','01919360','01919370','01941700','01941740','01954360','01956370','01956790','01960361','01961312','01965670','01992421','02167139','01020040','01984170','01021900','01020120','01028630','01030500','01097201','01107680','01121851','01132820','01133290','01142700','01143430','01148020','01154611','01154862','01157620','01159602','01159712','01167680','01169091','01196020','01238882','01890002','01913090','01925800','01939920','01944340','01975970','01026210')

select * from term_cluster


select * from terminal
where ter_tid = '01350740'

select * from terminal
where ter_tid in ('01515552','01521101','01521111','01521151','01521161','01521171','01919042','01546542','01574951','01577741','01577751','01591842','01622222','01521211')




update terminal
set TERMINAL.TER_MCC_TYPE = 'RESTAURANT'
where ter_tid in ('01515552','01521101','01521111','01521151','01521161','01521171','01919042','01546542','01574951','01577741','01577751','01591842','01622222','01521211')
commit

update terminal
set ter_cluster = 307
where ter_tid in ('01515552','01521101','01521111','01521151','01521161','01521171','01919042','01546542','01574951','01577741','01577751','01591842','01622222','01521211')
commit

SELECT XTP_TERMINAL, XTP_LOCK_TIME FROM EXTRA_T_PARAMETERS
where XTP_TERMINAL in ('01515552','01521101','01521111','01521151','01521161','01521171','01919042','01546542','01574951','01577741','01577751','01591842','01622222','01521211')


UPDATE EXTRA_T_PARAMETERS SET XTP_LOCK_TIME ='0600'
WHERE XTP_TERMINAL IN (SELECT TER_TID FROM TERMINAL WHERE TER_CLUSTER=307 and ter_tid in ('01515552','01521101','01521111','01521151','01521161','01521171','01919042','01546542','01574951','01577741','01577751','01591842','01622222','01521211') )
commit