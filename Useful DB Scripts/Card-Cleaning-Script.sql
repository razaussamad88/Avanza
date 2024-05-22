
/*---------------------------------------------------------*/
/* DISCARD CARD PRODUCTS  */

Delete From DEBIT_CARD_TYPE
where CARD_TYPE_CODE not in (
	Select distinct CARD_TYPE_CODE From DEBIT_CARD
)


Delete From DEBIT_CARD_PRODUCT_IMD
where DEBIT_PROD_CODE not in (
	Select distinct DEBIT_PROD_CODE From DEBIT_CARD
)


Delete From DEBIT_CARD_PRODUCT
where DEBIT_PROD_CODE not in (
	Select distinct DEBIT_PROD_CODE From DEBIT_CARD Union All
	Select distinct DEBIT_PROD_CODE From DEBIT_CARD_TYPE
)



/*---------------------------------------------------------*/
/* DISCARD PERSONALIZED CARDS  */

-- Reset Approved Personalize Cards
Update CARD_REQUEST_PERSONALIZED_LOG
Set APPROVAL_STATUS_CODE='002'
where RELATIONSHIP_NUM='10022';


-- Select * From #tmp_PPCards
-- drop table #tmp_PPCards
SELECT PAN,ACCOUNT_NUM,RELATIONSHIP_NUM
INTO #tmp_PPCards
From DEBIT_CARD_ACCOUNT
where PAN in (
	Select PAN From DEBIT_CARD
	where IS_PREPAID_CARD=1 
	and RELATIONSHIP_NUM <> 'C000000000'
	and CARD_TITLE <> 'PREPAID CARD'
);


Delete From DEBIT_CARD_ACCOUNT
where PAN in (
	Select PAN From #tmp_PPCards
);


Delete From CUSTOMER_ACCOUNT
where ACCOUNT_NUM in (
	Select ACCOUNT_NUM From #tmp_PPCards
);


Delete From ACCOUNT
where ACCOUNT_NUM in (
	Select ACCOUNT_NUM From #tmp_PPCards
);


Delete From CUSTOMER_CHANNEL_AUTHEN
where CUST_CHANNEL_ID in (
	Select PAN From #tmp_PPCards
);


Delete From DEBIT_CARD
where PAN in (
	Select PAN From #tmp_PPCards
)

/*---------------------------------------------------------*/
