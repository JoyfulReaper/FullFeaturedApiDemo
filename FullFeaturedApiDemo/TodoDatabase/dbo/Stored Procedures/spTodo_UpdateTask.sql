CREATE PROCEDURE [dbo].[spTodo_UpdateTask]
	@Task NVARCHAR(300),
	@OwnerId NVARCHAR(450),
	@TodoId INT
AS
BEGIN
	UPDATE dbo.[Todo]
		SET Task = @Task
	WHERE
		TodoId = @TodoId
	AND
		OwnerId = @OwnerId
END