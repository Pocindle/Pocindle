UPDATE users
SET pocketaccesstoken = @AccessToken
WHERE users.pocketusername = @PocketUsername;