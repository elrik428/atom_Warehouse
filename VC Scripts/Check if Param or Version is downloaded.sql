
---   DOWNLOADED
select distinct vc30.relation.termid as TID, a.[value], b.[value] , c.[value] , d.[value], e.[value] ,
vc30.TERMLOG.EVDATE as DOWNLOADED,
vc30.TERMLOG.famnm,vc30.TERMLOG.message,vc30.TERMLOG.status,vc30.TERMLOG.appnm, vc30.TERMLOG.duration
from (vc30.relation left join vc30.TERMLOG on
      vc30.relation.TERMID=vc30.TERMLOG.TERMID
      left join vc30.PARAMETER a on vc30.relation.TERMID=a.PARTID and a.PARNAMELOC ='MERCHID'
      left join vc30.PARAMETER b on vc30.relation.TERMID=b.PARTID and b.PARNAMELOC='RECEIPT_LINE_1'
      left join vc30.PARAMETER c on vc30.relation.TERMID=c.PARTID and c.PARNAMELOC='RECEIPT_LINE_2'
      left join vc30.PARAMETER d on vc30.relation.TERMID=d.PARTID and d.PARNAMELOC='RECEIPT_LINE_3'
      left join vc30.PARAMETER e on vc30.relation.TERMID=e.PARTID and e.PARNAMELOC='RECEIPT_LINE_4'
      )
where
      (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
      vc30.TERMLOG.appnm like '%PIRA0203P%' and
      vc30.TERMLOG.message = 'Download Successful' and
      vc30.TERMLOG.EVDATE > '2018-05-04 00:00:00.000' and
      vc30.TERMLOG.TERMID in
   ('00011977','00096771','00204191','00226275','00241252','01000553','01000640','01001126','01002430','01003340','01006200','01008180','01009160','01010180','01010610','01011290','01012060','01013060','01013130','01013150','01013324','01014520','01014730','01015790','01016340','01016580','01027241','01027774','01028260','01031775','01031990','01032032','01033601','01033830','01034930','01035031','01035932','01038620','01041250','01042480','01043630','01044570','01046560','01046600','01046810','01047140','01049050','01049880','01051600','01052940','01054671','01055830','01056260','01056650','01059660','01061370','01061480','01062073','01062350','01062371','01063530','01063904','01063950','01065450','01066490','01067120','01070042','01070810','01071820','01073490','01075365','01076100','01078921','01078934','01078953','01079041','01079062','01079550','01079951','01080390','01083551','01085100','01085190','01088641','01088910','01089683','01090010','01090822','01092301','01093040','01093100','01093151','01093500','01096650','01097640','01107410','01107901','01115021','01116790','01117084','01123560','01132878','01139811','01141262','01141343','01153003','01160800','01163710','01165112','01173481','01174281','01175913','01182363','01183470','01184222','01184900','01187421','01187471','01187590','01187720','01191010','01192841','01193821','01197360','01197706','01203342','01203541','01212402','01212702','01215160','01215301','01215740','01216011','01216041','01216481','01216860','01219030','01220080','01223090','01224011','01226001','01227440','01233920','01233975','01238002','01240581','01240612','01242732','01243280','01243720','01243981','01244540','01249861','01250000','01253991','01254226','01255012','01255912','01256151','01256220','01257070','01257542','01257551','01257561','01259741','01266321','01266540','01268300','01268640','01271270','01271970','01272122','01275644','01278171','01278940','01280732','01280910','01282203','01282861','01287041','01287152','01288542','01289821','01289953','01291440','01291791','01298230','01300842','01300950','01301160','01303670','01304042','01304210','01308601','01309140','01309621','01310361','01311491','01314760','01315891','01317250','01318880','01320542','01321602','01323640','01324861','01325821','01326420','01327331','01327501','01328450','01331321','01332421','01335660','01337681','01339110','01339640','01342640','01343330','01344480','01346530','01348460','01357780','01359442','01360112','01360282','01361360','01362190','01363611','01363671','01363881','01365201','01366090','01366440','01367130','01367491','01370390','01371160','01371440','01371860','01371901','01373041','01373511','01373640','01375341','01376861','01377152','01378651','01379980','01380081','01382843','01383790','01385560','01388550','01389760','01395001','01395021','01397271','01400862','01402551','01403890','01404221','01405330','01405550','01406191','01407831','01409671','01410171','01412120','01413310','01415150','01419910','01421680','01424350','01425091','01425170','01428240','01428670','01431130','01436280','01438650','01439300','01440180','01440350','01441310','01442830','01443350','01449590','01451680','01452912','01454870','01455790','01458054','01463141','01463152','01465291','01465840','01466791','01467310','01471000','01473561','01475280','01475852','01476711','01477070','01478350','01479880','01480581','01480583','01482570','01482852','01483882','01487210','01487850','01488281','01488440','01489161','01489311','01490080','01490271','01491370','01491500','01491541','01492781','01493590','01494200','01494960','01498621','01500060','01500221','01501780','01501990','01502940','01503350','01503400','01504896','01506441','01506981','01507060','01509940','01510142','01510460','01510921','01511701','01512160','01512620','01513150','01513660','01515460','01516120','01516210','01516830','01525770','01526100','01526190','01526460','01527890','01529280','01530990','01531610','01533710','01534220','01535380','01536670','01536880','01537150','01539740','01539830','01541820','01544601','01545330','01547710','01548480','01549210','01549653','01550650','01551340','01553400','01554561','01555581','01559430','01560041','01560862','01561721','01563250','01564321','01564630','01569350','01569450','01571060','01572990','01573320','01573631','01573750','01574710','01575860','01576580','01581420','01586680','01588230','01590901','01594170','01594791','01597090','01597170','01598470','01601110','01606750','01609040','01609670','01610020','01610200','01612651','01614930','01616290','01616830','01618710','01618721','01620000','01620870','01621571','01621821','01622080','01624790','01625220','01626020','01626630','01627350','01627660','01627770','01628770','01630190','01630590','01632940','01639422','01642410','01642840','01643892','01645061','01645570','01645630','01646460','01646974','01649280','01649710','01653450','01653700','01654260','01654320','01654410','01654860','01657700','01657910','01658551','01660240','01662120','01664750','01664900','01666215','01666780','01667020','01668670','01669230','01676130','01680891','01681790','01684400','01684822','01684970','01684981','01686660','01687810','01689420','01692101','01692250','01692911','01695541','01696001','01699430','01700070','01701961','01703000','01706780','01706951','01709042','01709520','01709530','01713730','01714720','01715780','01715980','01716185','01716590','01717750','01719563','01719900','01720780','01721320','01722270','01722910','01723170','01727870','01728630','01730910','01732530','01732830','01737081','01737180','01737390','01737810','01739330','01741870','01741920','01742011','01744431','01746060','01746350','01747560','01748252','01748260','01749811','01750940','01754490','01755020','01755530','01756120','01756300','01757410','01757700','01758681','01759810','01760070','01760311','01760390','01761270','01761900','01761940','01763920','01764470','01765230','01765412','01765420','01767790','01769080','01769146','01769701','01769760','01770370','01771930','01772110','01773580','01774452','01774680','01774800','01775810','01775870','01776150','01777320','01777811','01778820','01780700','01781070','01781211','01781241','01782743','01782802','01782912','01785020','01785090','01786200','01788910','01790320','01790611','01790851','01791080','01792390','01793851','01795331','01795610','01796110','01798430','01799181','01799331','01800150','01800861','01805011','01807240','01807570','01809060','01809150','01811531','01813230','01813350','01816290','01816741','01818681','01819160','01824470','01825330','01825880','01826300','01826570','01827170','01827710','01829310','01831140','01831150','01833730','01835170','01836180','01836711','01839400','01839410','01839931','01839940','01840350','01841020','01841730','01842910','01843070','01843680','01843770','01845250','01846280','01846451','01847920','01848190','01849160','01850210','01850690','01851050','01851850','01852250','01852330','01854424','01854710','01855991','01856230','01857480','01858030','01858190','01858592','01860370','01861061','01861070','01861160','01861781','01864110','01864811','01867891','01868381','01868711','01869540','01870350','01870510','01871240','01872360','01872380','01874260','01874500','01874770','01875220','01878342','01881410','01882891','01884031','01884281','01884640','01884840','01884951','01886140','01886911','01892562','01892832','01894832','01895431','01896491','01896620','01896700','01896900','01897511','01898350','01898370','01898890','01900251','01900770','01901230','01902150','01902250','01902590','01902961','01903240','01903592','01903770','01904191','01904370','01904440','01905900','01906021','01907691','01908872','01909760','01910630','01910831','01910940','01911190','01913341','01913371','01914432','01914590','01916112','01916513','01916652','01916771','01916861','01917221','01917672','01918842','01920152','01932570','01941350','01946370','01957030','01957042','01970061','01977080','01977670','01978770','01979630','01980240','01982920','01986690','01987450','01987840','01989460','01990360','01991270','01993300','01994020','01994660','01994780','01994960','01995380','01997450','01997730','01998780','02273125','19811038','19811132','19811151','19811325','19811498','19811664','19811688','19811759','19811794','19811799','19811804')



   ---  NOT DOWNLOADED
   select distinct vc30.relation.termid as TID
   from  vc30.relation
   where vc30.relation.clusterid = 'EPOS_DUTYFREE'
   and vc30.relation.termid not in ( select vc30.TERMLOG.TERMID from vc30.TERMLOG where
         (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
         vc30.TERMLOG.appnm like '%EPOS0213P%' and
         vc30.TERMLOG.message = 'Download Successful' and
         vc30.TERMLOG.EVDATE > '2018-09-24 16:00:00.000')


-- For Donna
         select distinct q.PARTID as TID
            from  (SELECT [PARTID]  FROM [vc30].[vc30].[PARAMETER]
         		  where parnameloc='services_enabled' and [value]='TRUE' and partid not like '73%'
         		   and partid not in ('520CHAL','520CHALREC','520KOTSO','675CHALREC','KOTSMCCT','KOTSO675','KOTSTST','8508TEST','TESTDONN','520EPPCAR','520EPPHOT','520EPPINST','520EPPRES','520EPPSAL','520EPPSUP','520EPPTRA','520GSERV','675EPPCAR','675EPPHOT','675EPPINST','675EPPRES','675EPPSAL','675EPPSUP','675EPPTRA','COMBOSERV','TESTDON2')) q
            where q.PARTID not in
            (select vc30.TERMLOG.TERMID from vc30.TERMLOG
         	where (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
         	 vc30.TERMLOG.message = 'Download Successful' and
         	 vc30.TERMLOG.EVDATE > '2018-11-10 11:00:00.000')


--For Antonis
select distinct q.PARTID as TID
from  (select PARTID from vc30.PARAMETER where
		PARNAMELOC = 'SERVICES_ENABLED' and [value] = 'TRUE' and substring(partid,1,3) not in ('520','675','820','690','COM','KOT','TES') and substring(partid,1,5) not in ('8508T')) q
where q.PARTID not in  (select vc30.TERMLOG.TERMID from vc30.TERMLOG
         				where (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
         				vc30.TERMLOG.message = 'Download Successful' and
         				vc30.TERMLOG.EVDATE > '2018-12-11 11:00:00.000')


--

select distinct q.PARTID as TID
   from  (SELECT [PARTID]  FROM [vc30].[vc30].[PARAMETER]
     where parnameloc='services_enabled' and [value]='TRUE' and partid not like '73%'
      and partid not in ('520CHAL','520CHALREC','520KOTSO','675CHALREC','KOTSMCCT','KOTSO675','KOTSTST','8508TEST','TESTDONN','520EPPCAR','520EPPHOT','520EPPINST','520EPPRES','520EPPSAL','520EPPSUP','520EPPTRA','520GSERV','675EPPCAR','675EPPHOT','675EPPINST','675EPPRES','675EPPSAL','675EPPSUP','675EPPTRA','COMBOSERV','TESTDON2')) q
--(select distinct partid from  [vc30].[vc30].[PARAMETER]
--where partid in
-- ('00916484','01031332','01006683','01009602','01010903','01025312','01066414','01090310','73001148','02153250','00246632','00263322','01019712','01098124','01027422','01258993','02188770','01236982','01268393','73000931','01310135','01574592','02166760','02171720','01393983','01138147','01229259','01341952','73008070','01329163','01357472','01254023','01288593','73008071','01222401','01132112','01190643','73007343','73004787','73000908','73000904','73000905','73008089','73000906','73007823','01283856','01128624','01010872','01032872','73008043','01612902','01466322','01615742','01639512','01430882','01417872','01734962','01573424','02255092','01454111','01575222','01625752','02290121','73007403','73000881','73000889','73000885','01613352','01557073','01675272','01524302','02306220','01744572','01762332','01626672','01690512','73001207','02180300','01669292','01646382','01711453','01700872','01647622','01733072','02270730','01598092','01503442','01699412','01594252','01534152','01509632','01578414','01422572','01480543','01435992','02280239','01636035','01427962','01486292','01672892','01664502','01639822','01664482','01578032','01657214','01661392','01414292','01400072','01724582','01609082','01006742','01680772','01926972','00090841','00645601','01252174','01435932','01953753','73001209','73001210','01921852','73001204','02115490','73001319','01845032','01763962','01965753','01361454','01382844','01183461','01970673','01849701','01866262','73001216','01896532','02075540','01168124','02149350','01929303','02129840','01926683','02129647','01373874','01424122','01722482','01536792','02076000','01772612','01830972','02149370','73006333','01396775','73005887','73005885','02129610','73001267','73001264','73001266','01966993','73001226','73001228','73001220','73001229','73001233','73001234','73001236','73001268','73001235','73001222','73001224','73001230','73001213','73001214','73001215','73001260','73001270','73001276','73001274','73001282','01056772','73001242','73001241','73001255','73000724','02146250','73008048','01393982','73001251','73001249','73001272','73001271','73001273','73001278','73001279','73001252','73001239','73001285','73001284','73001247','73001243','73001261','73001259','73001314','73001310','73001292','73001304','73001308','02149280','02149110','73001331','02148890','73001349','73001323','73001340','73001339','73001344','73001346','73001322','73001342','73001338','73001298','73000873','73000866','73000864','73000868','73000867','73000869','73000912','73000910','73000935','73000937','73000933','73001289','73000914','73000923','73000917','73000919','73000921','73001317','73001318','73001326','73001328','73001316','73001288','01095210','73001290','01091400','73001291','01091423','01091440','73001345','01132111','01020413','73001330','73001332','73001315','73001311','73001336','01176410','02149400','02149240','02287040','73001299','01223782','01792172','73001321','73001327','73001325','02914013','01875852','02171760','02173570','01624812','02140520','02053730','73001303','01611022','01519093','01963563','02075440','02102420','02107970','01902082','02146090','01980322','02129720','02146120','02208230','02129790','01900282','01872562','01938123','01508272','73001307','73001313','73001334','73008088','73004804','01947102','73006176','73007883','73004806','73006789','73005204','73001180','73004791','73004789','73004793','73004833','73004830','73001199','73001196','73001168','73001170','73001172','73001159','73001153','73004797','73004795','73003495','73002793','73002790','73003097','73003759','73002555','73002553','73000725','73006302','73007551','73006897','73005407','73005409','73006899','73006905','73006903','73006901','73006907','73005405','73005366','73005379','73004870','73004868','73004866','73006352','73006272','73006274','73006276','73006230','73006228','73006226','73006354','73006259','73006310','73006329','73006327','73005413','73005411','73005948','73006298','73006290','73007527','73007523','73007531','73007525','73007445','73007437','73007502','73007532','73007530','73007439','73007458','73007438','73007500','73007587','73007589','73007585','73007474','73007586','73007588','73001366','73007787','73007789','73007791','73000920','73000922','73000938','73000936','01433302','01907762','01950843','02129750','02129490','02146180','02129650','73002745','73002749','73002747','73003496','73005944','73005942','73007492','01936422','01950883','01898122','01874472','02149290','02146230','73001358','73007926','73008044','73007681','00724391','73001482','73008017','00997051','01052172','73008018','02230614','02149440','01073935','01039925','01099312','01554242','01054692','73001147','01094631','00644201','00260261','01060372','01087302','01017582','01245862','01312693','01323892','01314683','01388392','01377133','73008080','01399443','01291392','01271202','01373223','01397223','01379882','01401262','01311672','01317673','01391433','02165400','01168123','01170432','01265031','01281504','01395912','73004788','01332832','02209160','01004784','01048302','01053242','01073053','01637012','02235580','01424982','01481772','01496542','01701522','01659732','01435162','02293489','02209210','01411632','01675863','01419942','01533562','01459562','01696122','01452364','01589931','01643172','01470792','01476902','01621214','01659052','01693082','01439522','02161170','01566462','01475702','01484602','02153840','73001202','73001203','01737622','01693671','01636372','01487262','01624232','01528202','02201590','01528822','02201610','01531162','01541652','01619482','01475912','73001205','73001206','01710553','01479472','01607283','01656113','02272400','01552933','01483262','01427862','01463523','01510512','01567182','01424552','01951133','01884572','01055732','01964622','01693282','01621512','01476472','02149120','00425271','01542453','01601452','01193013','01873822','73001208','73001201','01767363','02149300','01885875','73000916','73000918','02128850','01963782','02115630','02129560','01379812','02129660','01646392','02146320','01161774','73001212','01972053','73001232','73001231','01981033','73001223','01243322','73001219','02129540','02129820','01599190','73005744','73005886','73001396','73001263','73001269','73001265','73001218','73001221','73005977','73001225','73001211','73001217','73001277','73001256','73001246','73001245','73001254','73001253','01977812','73001280','73001275','73001281','73001244','73001250','73001258','01964922','73001238','73001240','01961162','01137806','73001237','73001283','73001248','73001287','73001286','73001262','73001294','73001300','73001302','73001306','73001296','73001297','73001335','73001354','73001352','73001356','73001295','73001333','73001337','73001320','73001324','73001293','73005990','73000865','73001301','73000863','73000879','73001357','73000883','73000877','73000875','73000940','73001104','73000896','73000893','73000898','73001124','73001122','73001341','73001343','73001120','73001118','73001116','73001347','73001312','73001108','73001106','73001114','73001110','01090840','01111480','73001348','73001350','01138142','01002401','01041850','73001353','73001355','73001351','02149130','01489962','02149250','01699402','01335822','02188800','02189860','02189910','02149390','01957922','01956044','02202580','02127740','01424892','01414774','02129780','02129600','02146190','01806162','02201620','01966532','01198562','01298825','02208440','02294818','02301041','02286950','02129760','02306180','01906792','02153310','02153340','02201600','01509882','01709822','73001305','01959645','01942893','73001309','01911172','73001363','73001361','73001360','73001362','73001387','01004933','01047850','73002773','01093601','73007552','73004805','73005203','73007456','73000880','73000878','73000884','73000886','73000890','73000876','73000874','73000882','73003757','73004785','73001136','73001134','73001532','73003755','73004860','73001132','73001130','73001126','73004809','73004814','73004816','73001162','01269021','73001174','73001176','73001178','73004864','73001155','73001157','73001165','73003360','01857192','01398674','73004776','73004779','73004782','73002748','73002750','73003095','73002746','73006766','73006791','73006009','73006007','73006005','73006294','73006793','73006795','73006797','73005935','73005417','73005415','73006297','73006291','73006299','73006301','73006303','73005599','73006295','73005540','73005943','73006293','73006300','73000913','73000911','73000924','73000899','73001117','73001105','73001107','73001109','73007529','73001115','73001111','73001119','73007476','73007478','73007401','73007369','73007399','73001103','73007995','73007590','73007854','73007996','73001181','01597382','01659212','02222050','02152110','02165430','02171730','01777232','02161230','02180670','02188750','01263321','01134682','01247912','01031571','01257840','73000943','73000932','73000895','73000897','73001359','73001125','73001123','73001121','01952302','73001169','73001131','73001135','73001154','01221291','02129710','73007764','73007235','73007326','01395332','01315782','01312163','73003756','73001133','73004810','73001171','73001129','73001160','73001167','73004813','73001156','73001158','73001164','73004836','73004831','73004790','73004792','73004861','73001531','73004786','73004784','73004815','73001179','73001177','73004796','73001364','73001141','73001200','73001198','73002792','73002794','73001380','73004775','73003758','73003094','73001388','73005367','73004865','73004867','73004869','73001368','73001372','73005884','73006790','73006767','73001383','73001384','73001385','73005408','73001394','73005406','73001392','73006275','73006260','73006696','73001393','73006898','73006048','73005791','73001375','73005902','73006231','73006229','73005905','73005541','73005903','73005600','73005418','73005414','73005416','73005964','73005971','73005969','73006328','01281512','73006008','73006904','01475792','01470102','02076050','73007338','73007330','73005973','73007311','73007340','73007321','73007683','73007760','73007762','01355872','02129740','01481422','01913882','73007759','02149340','73007317','73005999','73007459','73007528','73007526','01256802','01497902','73007347','73007766','73007339','73007337','01289194','01460522','01286303','73007355','73007557','01838032','73007763','73007319','01011393','73007341','73007313','01956553','73007346','73007402','01326973','01940633','01766292','73007334','73005982','73007354','73007329','73007344','73007314','73007370','73007236','73007348','73007404','73007322','73005995','73007324','73008001','73007820','73007840','73007788','73007792','73007922','73007817','73007827','73007900','73007904','73007906','73007896','73007923','73007925','73007897','73007779','73007924','73007767','73007993','73007994','73007775','73007771','73000961','73000964','73006691','73000722','73000719','73006011','73000721','73000720','01645802','02076010','01926073','01849071','01872582','73005979','73005985','02300991','01265631','01694424','02302010','73000970','73000977','73000972','73000975','73000971','73000973','73000976','73004811','73005957','73005432','73005783','73006010','73006326','73006015','73006278','73006687','73006340','73006162','73006166','73006164','73006168','73006688','73006690','02159320','02159260','73006012','73006253','73006686','73006264','73006692','73006163','73006171','73006169','73006167','73006165','73007364','73007366','73006837','73007462','73006799','73007464','73006895','73006709','73007367','73006697','73007359','73007361','02287050','02129860','02118860','73007360','73007463','73007465','73007365','73007409','73007413','73007407','73006833','73006822','73007412','73007418','73007416','73007408','73007425','73007310','73007308','73007997','73007415','73007419','73007307','73006764','01991482','73007467','73007479','73007469','73007961','73007746','73007470','73007506','02075450','73007556','73007742','02149100','01959734','02300980','02277470','01946573','02294826','01863982','73005993','73005986','01264213','02159250','02158520','73005974','02204400','01603792','01734012','01970682','01570312','01527122','01427772','02102410','01452962','01931593','01965201','02075580','02129630','02159310','02053680','01625982','01970312','02107900','01323732','02189800','02189870','02202560','01747562','01762722','02201510','73005975','73005994','73006002','73006001','01398670','01429040','01296931','01469600','01373241','01376380','01685340','02153330','02158410','01674710','02158540','02171750','01694422','01907790','01768410','02185830','02149510','01614342','01652500','01657211','01564920','01659260','01731220','01870260','01753420','01528220','01537880','01754790','01691150','01865250','01766070','01872560','01575240','73008084','01008371','01233991','01054690','01097353','01032870','01044520','01100012','01049110','01248730','01069070','01227130','01036982','01250251','01077681','01246271','01087440','01089741','01094860','01098122','00177951','01100530','01118961','01000362','01067821','01140570','01248121','01251062','01267161','01468060','01489960','01497250','01469080','01369880','01509670','01285071','01417300','01388880','01464500','01484690','01427390','01319031','01333770','01482060','01464450','01312701','01317672','01470231','01463520','01322772','01429740','01431580','01506470','01470100','01482441','01516180','01362750','01433170','01432390','01493520','01410800','01411630','01352540','01509260','01388390','01418010','01438670','01440950','73004817','73004863','01182052','73001373','73004794','73001173','73001369','73001175','73001371','73004808','73001365','73001376','73001378','73001382','73003361','73004781','73004783','73004778','73003096','73001389','73001391','73001386','73002554','73006785','73001370','73001367','73006792','73006794','73006796','73005404','73005410','73006351','73001377','73006227','73006309','73001381','73001379','73001374','73001395','73005412','73005970','73005967','73005965','02129590','01996843','73006353','73005972','73005963','73006365','73006271','73006273','73006006','73005904','01692803','73005968','73006900','73006902','01952593','73006906','73007312','73007238','73005966','73007234','73007400','73006943','73007473','73007628','73007682','02146330','73007331','02112530','02277430','02171180','02149380','73007524','73007446','01312703','73007315','73007237','01389462','73007550','73007758','01410802','73007357','73007335','01231613','73005976','73007345','73007349','73007351','73007333','01311284','73007323','73007327','73007475','73007325','01959223','01421892','73007353','73007328','02129800','73007318','73005980','73007342','01474872','02265160','73007316','73007356','73007765','73007477','73007350','73005987','73007358','73007905','73007902','73007352','73007332','73007336','73007320','73007841','73007826','73007855','73007774','73007839','73007818','73007824','73007828','73007773','73007822','73007790','73007819','73007825','73007768','73007770','73007776','73007772','73007990','73007829','73007998','73007999','73007901','73007960','73007769','73007903','73007907','73000962','73000960','73005989','73000723','02129730','02149360','02286990','73000969','73000966','73005208','73004838','73004837','73004862','73005958','73005433','73005984','73005988','02129550','73005996','73006014','73006689','73006247','02201580','73006170','73006277','73006013','73006223','73006224','73006248','73006254','73006330','73006016','73006161','73006894','73006765','73006751','73007363','73005991','73007444','73006708','73006703','73006693','73007460','73006706','73006695','73006704','73006707','02149260','01936963','02129830','01954813','02115542','02127920','73007466','73006838','73007368','73007414','73007411','73007362','73007420','73007410','73007309','02219460','73007872','73007417','73007421','73006834','73007962','73007468','73007554','02293471','02053800','73007874','73007873','73007877','01872442','73007875','02208330','73007876','73007761','73007555','02152090','02152120','01329164','02265190','02261560','01851502','02180680','01503440','01412672','01439882','73005983','01458872','02129570','01976033','02185790','02173630','02188790','01711052','02146290','01274861','01436672','01464503','01892374','02221500','01737452','01418622','73005997','73005978','73005992','73005981','73006000','01741282','01675862','02149530','01613122','01654262','01578030','01844510','01785230','01735431','01886260','01950841','01952461','01956183','01947481','01664500','01573340','01550410','01643170','01758390','01714640','01818370','01926681','01781190','01587540','01545170','01534380','01982690','40000011','01722000','01536530','01944331','01675270','73008045','73008046','01569710','01580210','01859660','01073051','01085581','01238981','01003100','01128330','73008085','73008086','73008087','73000907','73000934','73000968','73008090','73008091','01091390','01393980','01344290','01423490','01422680','01357470','01329141','01495420','01513200','01502670','01470520','01430340','01416880','01513620','01435990','01420360','01391431','01424290','01509630','01439100','01319820','01436460','01436290','01428450','01307490','01476900','01303540','01415070','01319850','01373200','01424800','01278411','01375530','01432160','01504340','01309365','01395330','01335320','01475700','01507880','01341950','01669751','01802771','01741470','01853240','01903730','01723670','40000023','01542120','01871680','01859810','01692762','01954811','01856801','01821450','01803030','01651810','01936951','40000010','01938121','01661200','01983410','01898950','01727170','01902630','01925011','01996240','01834070','01556050','01668260','01624810','01729670','01690510','01857442','01948971','01841470','01711250','40000024','01719220','01720150','01632643','40000034','01909630','01598091','01857051','01727730','01675860','01546170','01764350','01797430','01837180','01875510','01861550','01959611','01612380','01613340','01717870','01908032','01963561','01906032','01842370','01724390','01597910','01621380','01687030','01626670','01680770','01679660','01625500','01968511','01699140','01873820','01548680','01642531','01957611','01666400','01655920','01666821','01867701','01243321','01163380','01229634','01259711','01240601','40000436','40000437','40000337','40000338','40000425','40000341','40000342','40000345','40000343','40000344','40000359','40000464','40000458','40000454','40000465','40000462','40000448','40000398','40000449','40000283','40000453','40000455','40000387','40000389','40000388','40000427','40000397','40000407','40000466','40000468','40000469','40000176','40000170','40000440','40000444','40000162','40000434','40000124','40000197','40000175','40000191','40000165','40000163','40000059','40000232','40000227','40000441','40000268','40000350','40000439','40000401','40000461','40000450','40000375','40000376','40000460','40000452','01488610','01416060','01279861','01306511','01407940','01421360','01354710','01516260','01502680','01468400','01328630','01420310','01414590','01469930','01323960','01361452','01309990','01357970','01285221','01314681','01315570','01315780','01318810','01321761','01333151','01373220','01487920','01438470','01460151','01512320','01525590','01525930','01479320','01359010','01396400','01371880','01401260','01469090','01376330','01386360','01415810','01389460','01410172','01417440','01424980','01437420','01335820','01482140','01482950','01441170','01367312','01440560','01470790','01497450','01283041','01322540','01434290','01548280','01947101','01851221','01909221','01835030','01589480','01647510','01607590','01909170','01902080','01888800','01951131','01962991','01961921','01683570','01692781','01655890','01617220','40000040','01977560','01631220','01757140','01692791','01692801','01692741','01982050','01652650','01646110','01950541','01533280','01580260','01789930','01943041','01946441','01957651','01764960','01861780','01957291','01961571','01959241','01976881','01606040','01593510','01575220','01535850','01588030','01755260','01927521','01927861','01996340','01997050','01540590','01952041','01909191','01772611','01955611','01580800','01770880','01901900','01823790','01844290','01755830','01673480','01772650','01661080','01754800','01700870','01639820','01860730','01937721','01841450','01557191','01729350','40000008','01613360','01666750','01642530','01666940','40000049','01560522','01863981','40000053','40000046','40000048','40000041','01675230','01552930','01929981','01534470','01652330','01689631','40000276','40000066','40000323','40000364','40000128','40000360','40000410','40000198','40000281','40000061','40000301','40000269','40000208','40000351','40000203','40000177','40000399','40000307','40000377','40000233','40000289','40000428','40000257','40000249','40000403','40000390','40000217','40000146','40000148','40000078','40000131','40000240','40000215','40000219','40000205','40000069','40000075','40000224','40000225','40000071','40000063','40000380','40000357','40000298','40000243','40000378','40000379','40000381','40000221','02149450','02149520','02153850','02185582','02158420','02158530','02158550','01458870','01525900','01435160','01433850','01498080','01195250','01256981','01062770','01055730','01264211','01091430','01137801','01092900','01030871','01107311','01238211','01195111','01131598','00238471','01080201','01089751','01090560','01258611','01112941','01010140','01701060','01555600','01954142','01732322','01996842','01547320','01542640','01853230','01939511','01944531','01942921','01712101','01939351','01657471','01538030','01653090','01541440','01525941','01531070','01532300','01942151','01957051','01742640','01958321','01574740','01654110','40000031','01529370','01692752','01925020','01925001','01924991','01647620','01993740','01951891','01987200','40000033','40000039','01566490','01553260','01592320','01691850','01927531','01930092','01997130','01996651','01587780','01559540','01806160','01200922','01233731','00008716','01011391','40000474','40000472','40000475','40000476','40000300','40000302','40000423','40000282','40000121','40000248','40000254','40000264','40000386','40000226','40000406','40000396','40000206','40000400','40000180','40000270','40000260','40000363','40000055','40000373','40000181','40000157','40000319','40000335','40000433','40000278','40000358','40000065','40000336','40000126','40000286','40000438','40000303','40000304','40000321','40000424','40000442','40000470','01254172','01028381','01009630','01070460','01048880','01083180','01091412','01016213','01518440','01368810','01419940','01318520','01341510','01427770','01439190','01342080','01481050','01435490','01460520','01468980','01433250','01421210','01404950','01411130','01352460','01428350','01424720','01430080','01437970','01423460','01436630','01364240','01418750','01471220','01465200','01501190','01436300','01452510','01470170','01509130','01411690','01522800','01432740','01509390','01353461','01321332','01288591','01309891','01428951','01436240','01503610','01464070','01474120','01431120','01519050','01521760','01365022','01312691','01873790','01990361','40000020','01539390','01952300','01892592','01928231','01887780','01767280','01616580','01624010','01787240','01706900','01711450','01716160','01733681','02973408','01943851','01859610','01630620','01956661','01936421','01821640','01930631','40000022','01911451','01684450','01624340','01800130','01862671','01691060','01623430','01680400','01718630','01530000','01655350','01612900','01531160','01874470','01676380','40000052','40000050','40000054','01682130','01936961','01684990','01965751','01980480','01976880','01574590','01813130','01544211','01614890','40000003','01782680','01692731','01722480','01819710','01692771','01571170','01565021','01587750','01850810','01871690','01573580','01898120','01733930','01951111','01578120','01657510','01641390','01715050','01598010','01536790','01761670','01991480','01750190','01967131','01704110','01712890','01703790','01619860','01617600','40000234','40000237','40000238','40000239','40000214','40000216','40000223','40000073','40000297','40000299','40000325','40000252','40000154','40000156','40000120','40000130','40000086','40000077','40000080','40000412','40000422','40000293','40000415','40000308','40000419','40000421','40000318','40000417','40000420','40000324','40000326','40000327','40000418','40000429','40000329','40000258','40000330','40000262','40000334','40000432','40000361','40000362','40000368','40000385','40000405','40000404','40000365','40000366','40000367','40000369','40000370','40000371','40000332','40000382','40000383','40000384','40000392','40000391','40000394','40000393','40000395','40000253','40000290','40000291','40000178','40000204','40000153','40000155','40000084','40000277','02185820','02188780','02235571','02235710','02209170','02290091','02290139','02149540','02153240','02178420','02270780','02280130','01039491','01014000','01260201','01269020','01004782')) q
 where q.PARTID not  in
   (select vc30.TERMLOG.TERMID from vc30.TERMLOG
 where (len(vc30.TERMLOG.appnm) > 9 or len(vc30.TERMLOG.appnm) = 0) and
  vc30.TERMLOG.message = 'Download Successful' and
  vc30.TERMLOG.EVDATE > '2019-03-18 11:00:00.000')
