namespace FortressCodesDomain.DbModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FortressCodeContext : DbContext
    {
        public FortressCodeContext()
            : base("name=FortressCodeContext")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<ChannelType> ChannelTypes { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceLevel> DeviceLevels { get; set; }
        public virtual DbSet<Family> Families { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<MembershipTier> MembershipTiers { get; set; }
        public virtual DbSet<PartnerChannel> PartnerChannels { get; set; }
        public virtual DbSet<Partner> Partners { get; set; }
        public virtual DbSet<PricingModel> PricingModels { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Tier> Tiers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<UserProfileInfo> UserProfileInfoes { get; set; }
        public virtual DbSet<VoucherMetadata> VoucherMetadatas { get; set; }
        public virtual DbSet<Underwriter> Underwriters { get; set; }
        public virtual DbSet<PartnerProgramme> PartnerProgrammes { get; set; }


        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<VoucherType> VoucherTypes { get; set; }
        public virtual DbSet<VoucherSKU> VoucherSKUs { get; set; }
        public virtual DbSet<SKUVolumePricing> SKUVolumePricing { get; set; }
        public virtual DbSet<tbl_PreloadedDevice> tbl_PreloadedDevices { get; set; }

        public virtual DbSet<tbl_VoucherRegistration> tbl_VoucherRegistrations { get; set; }
        public virtual DbSet<tbl_DeviceValueAudit> tbl_DeviceValueAudits { get; set; }
        public virtual DbSet<tbl_Profanity> tbl_Profanitys { get; set; }

        public virtual DbSet<PartnerPortalBusinessEntity> PartnerPortalBusinessEntities { get; set; }
        public virtual DbSet<ClaimsPortalUser> ClaimPortalUsers { get; set; }
        public virtual DbSet<ClaimRoles> ClaimRoles { get; set; }

        public virtual DbSet<FFS_InvoiceHeader> FFS_InvoiceHeader { get; set; }
        public virtual DbSet<FFS_InvoiceLine> FFS_InvoiceLine { get; set; }
        public virtual DbSet<FFS_InvoiceLineBatch> FFS_InvoiceLineBatch { get; set; }

        public virtual DbSet<RetailClass> RetailClass { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.PartnerChannels)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.partnerid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Device>()
                .HasMany(e => e.DeviceLevels)
                .WithRequired(e => e.Device)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Level>()
                .HasMany(e => e.DeviceLevels)
                .WithRequired(e => e.Level)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Partner>()
                .HasMany(e => e.DeviceLevels)
                .WithOptional(e => e.Partner)
                .HasForeignKey(e => e.PartnerId);

            modelBuilder.Entity<Partner>()
                .HasMany(e => e.PricingModels)
                .WithOptional(e => e.Partner)
                .HasForeignKey(e => e.PartnerId);

            modelBuilder.Entity<Partner>()
                .HasMany(e => e.Tiers)
                .WithOptional(e => e.Partner)
                .HasForeignKey(e => e.PartnerId);

            modelBuilder.Entity<Partner>()
                .HasMany(e => e.CountryDefaults)
                .WithOptional(e => e.DefaultPartner)
                .HasForeignKey(e => e.DefaultPartnerId);

            modelBuilder.Entity<PartnerProgramme>()
               .HasMany(e => e.Partners)
               .WithOptional(e => e.PartnerProgramme)
               .HasForeignKey(e => e.PartnerProgrammeId);

            modelBuilder.Entity<PricingModel>().Property(o => o.DailyPrice).HasPrecision(18,9);

            modelBuilder.Entity<Tier>()
                .HasMany(e => e.PricingModels)
                .WithOptional(e => e.Tier)
                .HasForeignKey(e => e.TeirId);

            modelBuilder.Entity<Tier>()
                .HasMany(e => e.Vouchers)
                .WithOptional(e => e.Tier)
                .HasForeignKey(e => e.membershiptierid);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.ExceptionMessage)
                .IsFixedLength();

            modelBuilder.Entity<UserProfileInfo>()
                .HasMany(e => e.AspNetUsers)
                .WithOptional(e => e.UserProfileInfo)
                .HasForeignKey(e => e.UserProfileInfo_Id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Voucher>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.Voucher)
                .HasForeignKey(e => e.CodeId);

            modelBuilder.Entity<PartnerPortalBusinessEntity>()
                .HasMany(e => e.PartnerPortalBusinessEntityChildren)
                .WithOptional(e => e.PartnerPortalBusinessEntityParent)
                .HasForeignKey(e => e.ParentBusinessId);

            modelBuilder.Entity<PartnerPortalBusinessType>()
                .HasMany(e => e.PartnerPortalBusinessEntities)
                .WithRequired(e => e.PartnerPortalBusinessType)
                .HasForeignKey(e => e.BusinessTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PartnerPortalBusinessType>()
                .HasMany(e => e.PartnerPortalBusinessType1)
                .WithOptional(e => e.PartnerPortalBusinessType2)
                .HasForeignKey(e => e.ParentBusinessTypeId);

            //modelBuilder.Entity<tbl_PreloadedDevice>()
            //    .HasRequired(e => e.Voucher)
            //    .WithMany(e => e.PreloadedDevices)
            //    .HasForeignKey(e => e.VoucherID);

            modelBuilder.Entity<FFS_InvoiceHeader>()
                .HasMany(e => e.FFS_InvoiceLine)
                .WithRequired(e => e.FFS_InvoiceHeader)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FFS_InvoiceLine>()
                .HasMany(e => e.FFS_InvoiceLineBatch)
                .WithRequired(e => e.FFS_InvoiceLine)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RetailClass>()
                .HasMany(e => e.PricingModel)
                .WithOptional(e => e.RetailClass)
                .HasForeignKey(e => e.RetailClassId);

        }


        /// <summary>
        ///   FJT - Database sequence for unique voucher batch number
        /// </summary>
        /// <returns>next batch number</returns>
        public int GetNextVoucherBatchSequence()
        {
            return Database.SqlQuery<int>("SELECT NEXT VALUE FOR dbo.seqVoucherBatch;").First();
        }
    }
}
