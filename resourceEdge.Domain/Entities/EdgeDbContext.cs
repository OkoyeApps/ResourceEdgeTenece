﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace resourceEdge.Domain.Entities
{
    public partial class EdgeDbContext : DbContext
    {
        public EdgeDbContext()
            : base("name=ResourceDbContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Configuration.LazyLoadingEnabled = true;
            modelBuilder.Conventions.Remove(new PluralizingTableNameConvention());
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            //modelBuilder.Entity<ApplicationUserRole>().HasKey(x => new { x.UserId, x.RoleId });
            //modelBuilder.Entity<ApplicationRole>()
            //         .HasMany(e => e.AspNetUsers)
            //         .WithMany(e => e.AspNetRoles)
            //         .Map(m => m.ToTable("AspNetUserRole").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<ApplicationUserLogin>().HasKey(x=> new { x.LoginProvider, x.ProviderKey, x.UserId });
            modelBuilder.Entity<ApplicationUserRole>().HasKey(x => new { x.RoleId, x.UserId });
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>().ToTable("AspNetUser");

            //modelBuilder.Entity<IdentityUser>().ToTable("AspNetUser");
            //modelBuilder.Entity<IdentityRole>().ToTable("AspNetRole");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("AspNetUserRole");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaim");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("AspNetUserLogin");
            // throw new UnintentionalCodeFirstException();


            modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRole");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("AspNetUserRole");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("AspNetUserClaim");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("AspNetUserLogin");
        }
        public DbSet<AppUserClaim> AppUserClaim { get; set; }
        public DbSet<Claim> Claim { get; set; }
        public DbSet<EmployeeUnitDepartment> EmployeeBusinessUnitDepartment { get; set; }
        public DbSet<EmployeeConfiguration> EmployeeConfiguration { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<IdentityCode> IdentityCode { get; set; }
        public virtual DbSet<BusinessUnit> Businessunit { get; set; }
        public virtual DbSet<Department> departments { get; set; }
        public virtual DbSet<Jobtitle> Jobtitle { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Prefix> Prefix { get; set; }
        public virtual DbSet<EmploymentStatus> EmploymentStatus { get; set; }
        public virtual DbSet<products> products { get; set; }
        public virtual DbSet<ReportManager> ReportManager { get; set; }
        public virtual DbSet<EmpHoliday> EmpHoliday { get; set; }
        public virtual DbSet<EmployeeLeave> EmployeeLeave { get; set; }
        public virtual DbSet<LeaveManagement> LeaveManagement { get; set; }
        public virtual DbSet<LeaveManagementSummary> LeaveManagementSummary { get; set; }
        public virtual DbSet<LeaveRequest> LeaveRequest { get; set; }
        public virtual DbSet<MonthList> MonthList { get; set; }
        public virtual DbSet<Month> Month { get; set; }
        public virtual DbSet<WeekDay> WeekDay { get; set; }
        public virtual DbSet<Week> Week { get; set; }
        public virtual DbSet<EmployeeLeaveType> LeaveType { get; set; }
        public DbSet<Requisition> Requisition { get; set; }
        public DbSet<EmpPayroll> Payroll { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<Career> Career { get; set; }
        public DbSet<CareerPath> CareerPath { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<ActivityLog> ActivityLog { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<AppraisalMode> AppraisalMode { get; set; }
        public DbSet<AppraisalPeriod> AppraisalPeriod { get; set; }
        public DbSet<AppraisalRating> ApprasialRating { get; set; }
        public DbSet<AppraisalStatus> AppraisalStatus { get; set; }
        public DbSet<Ratings> Rating { get; set; }
        public DbSet<AppraisalConfiguration> AppraisalConfiguration { get;set;}
        public DbSet<AppraisalInitialization> AppraisalInitialization { get; set; }
        public DbSet<Parameters> Parameter { get; set; }
        public DbSet<SubscribedAppraisal> SubscribedAppraisal { get; set; }
        public DbSet<MailDispatcher> MailDispatcher { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<AppraisalQuestion> AppraisalQuestion { get; set; }
        public DbSet<AppraisalResult> AppraisalResult { get; set; }
        public DbSet<Candidate> Candidate { get; set; }
        public DbSet<CandidateWorkDetail> CandidateWorkDetail { get; set; }
        public DbSet<CandidateStatus> CandidateStatus { get; set; }
        public DbSet<InterviewStatus> InterviewStatus { get; set; }
        public DbSet<InterviewType> InterviewType { get; set; }
        public DbSet<CandidateInterview> CandidateInterview { get; set; }
        public DbSet<Interview> Interview { get; set; }
        public DbSet<Asset> Asset { get; set; }
        public DbSet<AssetCategory> AssetCategory { get; set; }
        public DbSet<RequestAsset> RequestAsset { get; set; }
        public DbSet<GeneralQuestion> GeneralQuestion { get; set; }
        public DbSet<Violation> Violation { get; set; }
        public DbSet<Consequence> Consequence { get; set; }
        public DbSet<DisciplinaryIncident> Discipline { get; set; }
        public DbSet<SystemAdmin> SystemAdmin { get; set; }
        public DbSet<Birthday> BirthDay { get; set; }
        public DbSet<AdditionalDetail> AdditionalDetail { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<CooperateCard> CooperateCard { get; set; }
        public DbSet<Dependency> Dependency { get; set; }
        public DbSet<Disability> Disability { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<JobHistory> JobHistory { get; set; }
        public DbSet<MedicalClaim> MedicalClaim { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<TrainingAndCertification> TrainingAndCertification { get; set; }
        public DbSet<Visa> Visa { get; set; }
        public DbSet<DependencyRelation> DependencyRelation { get; set; }
        public DbSet<EducationLevel> EducationLevel { get; set; }
        public DbSet<MedicalClaimType> MedicalClaimType { get; set; }

    }
    
}

