CREATE PROCEDURE [dbo].[spTodo_Delete]
	@OwnerId NVARCHAR(450),
	@TodoId INT
AS
BEGIN
	DELETE FROM dbo.[Todo]
	WHERE
		TodoId = @TodoId
	AND OwnerId = @OwnerId
END