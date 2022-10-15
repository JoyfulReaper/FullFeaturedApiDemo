CREATE PROCEDURE [dbo].[spTodo_Create]
	@Task NVARCHAR(250),
	@OwnerId NVARCHAR(450)
AS
BEGIN
	INSERT INTO dbo.[Todo]
		(Task,
		OwnerId)
	VALUES
		(@Task,
		@OwnerId)

	-- Select the newly inserted record
	SELECT
		[TodoId], 
		[Task],
		[IsComplete],
		[OwnerId]
	FROM
		dbo.[Todo]
	WHERE
		TodoId = SCOPE_IDENTITY();
END