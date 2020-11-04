namespace CHPOUTSRCMES.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_log_entry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LOG_ENTRY_TASK_T",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CallSite = c.String(),
                        Date = c.String(maxLength: 30),
                        Exception = c.String(),
                        Level = c.String(maxLength: 30),
                        Logger = c.String(maxLength: 100),
                        MachineName = c.String(maxLength: 300),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Thread = c.String(maxLength: 300),
                        Username = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Date, name: "SEARCH_INDEX");
            
            CreateIndex("dbo.LOG_ENTRY_T", "Date", name: "SEARCH_INDEX");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LOG_ENTRY_TASK_T", "SEARCH_INDEX");
            DropIndex("dbo.LOG_ENTRY_T", "SEARCH_INDEX");
            DropTable("dbo.LOG_ENTRY_TASK_T");
        }
    }
}
