SELECT users.userid, users.pocketusername, d.to, d.from, d.smtpserver, d.password
FROM users
         LEFT JOIN deliveryconfig d on d.id = users.deliveryconfigid
WHERE users.pocketusername = @PocketUsername;
