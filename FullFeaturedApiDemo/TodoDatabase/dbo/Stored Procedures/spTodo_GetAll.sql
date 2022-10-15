CREATE PROCEDURE [dbo].[spTodo_GetAll]
	@OwnerId NVARCHAR(450)
AS
BEGIN
	SELECT
		[TodoId], 
		[Task],
		[IsComplete],
		[OwnerId]
	FROM
		dbo.[Todo]
	WHERE
		OwnerId = @OwnerId;
END
