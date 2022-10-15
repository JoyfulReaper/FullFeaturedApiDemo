CREATE PROCEDURE [dbo].[spTodo_Complete]
	@OwnerId NVARCHAR(450),
	@TodoId INT
AS
BEGIN
	UPDATE dbo.[Todo]
		SET IsComplete = 1
	WHERE
		TodoId = @TodoId
	AND OwnerId = @OwnerId;
END