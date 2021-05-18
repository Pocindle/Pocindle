UPDATE users
SET kindleemailaddress = @KindleEmailAddress
WHERE users.userid = @UserId;
