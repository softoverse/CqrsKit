using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Softoverse.CqrsKit.Model.Entity;

public class BaseApprovalFlowPendingTask
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    #region Command

    [Required]
    public string CommandName { get; set; }

    [Required]
    public string CommandNamespace { get; set; }

    [Required]
    public string CommandFullName { get; set; }

    public long? CommandId { get; set; }

    #endregion Command

    #region Command Response

    public string? ResponseName { get; set; }
    public string? ResponseNamespace { get; set; }
    public string? ResponseFullName { get; set; }

    #endregion Command Response

    #region Command Handler

    public string? HandlerName { get; set; }
    public string? HandlerNamespace { get; set; }
    public string? HandlerFullName { get; set; }

    #endregion Command Handler

    #region Command ApprovalFlow Handler

    public string? ApprovalFlowHandlerName { get; set; }
    public string? ApprovalFlowHandlerNamespace { get; set; }
    public string? ApprovalFlowHandlerFullName { get; set; }

    #endregion Command ApprovalFlow Handler

    /// <summary>
    /// varbinary(max) to string in SQL Server =>
    /// declare @b varbinary(max)
    /// set @b = 0x5468697320697320612074657374
    /// select cast(@b as varchar(max))
    /// </summary>
    public byte[]? Payload { get; set; }

    public string? UniqueIdentification { get; set; }

    [Required]
    public ApprovalFlowStatus Status { get; set; }

    public byte[]? ReviewedBy { get; private set; }

    public string? CreatedBy { get; set; }

    [NotMapped]
    public IEnumerable<string> ReviewedByDecoded
    {
        get
        {
            return ReviewedBy == null ? new List<string>() : DecodeReviewedBy(ReviewedBy);
        }
    }

    public void AddReviewedBy(string value)
    {
        List<string> reviewedByList = DecodeReviewedBy(ReviewedBy);
        reviewedByList.Add(value);
        ReviewedBy = EncodeReviewedBy(reviewedByList);
    }

    public void RemoveReviewedBy(string value)
    {
        List<string> reviewedByList = DecodeReviewedBy(ReviewedBy);
        reviewedByList.Remove(value);
        ReviewedBy = EncodeReviewedBy(reviewedByList);
    }

    public void ClearReviewedBy()
    {
        List<string> reviewedByList = [];
        ReviewedBy = EncodeReviewedBy(reviewedByList);
    }

    private byte[] EncodeReviewedBy(List<string>? data) => Encoding.UTF8.GetBytes(string.Join(", ", data ?? new()));

    private List<string> DecodeReviewedBy(byte[]? data) => data == null || data.Length == 0 ? new() : Encoding.UTF8.GetString(data).Split(", ").ToList();
}

public enum ApprovalFlowStatus
{
    Pending = 0,
    Accepted = 1,
    Rejected = 2,
    Correction = 3
}