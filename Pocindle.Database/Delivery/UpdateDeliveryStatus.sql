UPDATE delivery
SET deliverystatus        = @DeliveryStatus,
    deliveryfailedmessage = @DeliveryFailedMessage
WHERE delivery.deliveryid = @DeliveryId;
