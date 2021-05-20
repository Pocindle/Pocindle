SELECT delivery.deliveryid,
       delivery.userid,
       delivery.articleurl,
       delivery.epubfile,
       delivery.mobifile,
       delivery.deliverystatus,
       delivery.deliveryfailedmessage,
       delivery."to"
FROM public.delivery
WHERE delivery.deliveryid = @DeliveryId;
