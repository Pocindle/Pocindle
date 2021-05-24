INSERT INTO public.delivery
VALUES (default, (SELECT users.userid FROM users WHERE users.pocketusername = @PocketUsername), @ArticleUrl, @Epubfile,
        @MobiFile, @DeliveryStatus, @DeliveryFailedMessage, @To)
RETURNING public.delivery.deliveryid