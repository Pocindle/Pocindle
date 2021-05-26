SELECT users.userid, users.pocketusername, users.kindleemailaddress
FROM Users
WHERE users.pocketusername = @PocketUsername;
