CREATE PROCEDURE [dbo].[spTodo_Get]
	@OwnerId NVARCHAR(450),
	@TodoId int
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
		OwnerId = @OwnerId
	AND TodoId = @TodoId
END