#region License
// 
// Copyright (c) 2007-2009, Sean Chambers <schambers80@gmail.com>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;
using FluentMigrator.Model;
using Moq;
using NUnit.Framework;

namespace FluentMigrator.Tests.Unit.Builders.Alter
{
    [TestFixture]
    public class AlterTableExpressionBuilderTests
    {
        [Test]
        public void CallingAsAnsiStringSetsColumnDbTypeToAnsiString()
        {
            VerifyColumnDbType(DbType.AnsiString, b => b.AsAnsiString());
        }

        [Test]
        public void CallingAsAnsiStringWithSizeSetsColumnDbTypeToAnsiString()
        {
            VerifyColumnDbType(DbType.AnsiString, b => b.AsAnsiString(255));
        }

        [Test]
        public void CallingAsAnsiStringSetsColumnSizeToSpecifiedValue()
        {
            VerifyColumnSize(255, b => b.AsAnsiString(255));
        }

        [Test]
        public void CallingAsBinarySetsColumnDbTypeToBinary()
        {
            VerifyColumnDbType(DbType.Binary, b => b.AsBinary(255));
        }

        [Test]
        public void CallingAsBinarySetsColumnSizeToSpecifiedValue()
        {
            VerifyColumnSize(255, b => b.AsBinary(255));
        }

        [Test]
        public void CallingAsBooleanSetsColumnDbTypeToBoolean()
        {
            VerifyColumnDbType(DbType.Boolean, b => b.AsBoolean());
        }

        [Test]
        public void CallingAsByteSetsColumnDbTypeToByte()
        {
            VerifyColumnDbType(DbType.Byte, b => b.AsByte());
        }

        [Test]
        public void CallingAsCurrencySetsColumnDbTypeToCurrency()
        {
            VerifyColumnDbType(DbType.Currency, b => b.AsCurrency());
        }

        [Test]
        public void CallingAsDateSetsColumnDbTypeToDate()
        {
            VerifyColumnDbType(DbType.Date, b => b.AsDate());
        }

        [Test]
        public void CallingAsDateTimeSetsColumnDbTypeToDateTime()
        {
            VerifyColumnDbType(DbType.DateTime, b => b.AsDateTime());
        }

        [Test]
        public void CallingAsDecimalSetsColumnDbTypeToDecimal()
        {
            VerifyColumnDbType(DbType.Decimal, b => b.AsDecimal());
        }

        [Test]
        public void CallingAsDecimalWithSizeSetsColumnDbTypeToDecimal()
        {
            VerifyColumnDbType(DbType.Decimal, b => b.AsDecimal(1, 2));
        }

        [Test]
        public void CallingAsDecimalStringSetsColumnSizeToSpecifiedValue()
        {
            VerifyColumnSize(1, b => b.AsDecimal(1, 2));
        }

        [Test]
        public void CallingAsDecimalStringSetsColumnPrecisionToSpecifiedValue()
        {
            VerifyColumnPrecision(2, b => b.AsDecimal(1, 2));
        }

        [Test]
        public void CallingAsDoubleSetsColumnDbTypeToDouble()
        {
            VerifyColumnDbType(DbType.Double, b => b.AsDouble());
        }

        [Test]
        public void CallingAsGuidSetsColumnDbTypeToGuid()
        {
            VerifyColumnDbType(DbType.Guid, b => b.AsGuid());
        }

        [Test]
        public void CallingAsFixedLengthStringSetsColumnDbTypeToStringFixedLength()
        {
            VerifyColumnDbType(DbType.StringFixedLength, e => e.AsFixedLengthString(255));
        }

        [Test]
        public void CallingAsFixedLengthStringSetsColumnSizeToSpecifiedValue()
        {
            VerifyColumnSize(255, b => b.AsFixedLengthString(255));
        }

        [Test]
        public void CallingAsFixedLengthAnsiStringSetsColumnDbTypeToAnsiStringFixedLength()
        {
            VerifyColumnDbType(DbType.AnsiStringFixedLength, b => b.AsFixedLengthAnsiString(255));
        }

        [Test]
        public void CallingAsFixedLengthAnsiStringSetsColumnSizeToSpecifiedValue()
        {
            VerifyColumnSize(255, b => b.AsFixedLengthAnsiString(255));
        }

        [Test]
        public void CallingAsFloatSetsColumnDbTypeToSingle()
        {
            VerifyColumnDbType(DbType.Single, b => b.AsFloat());
        }

        [Test]
        public void CallingAsInt16SetsColumnDbTypeToInt16()
        {
            VerifyColumnDbType(DbType.Int16, b => b.AsInt16());
        }

        [Test]
        public void CallingAsInt32SetsColumnDbTypeToInt32()
        {
            VerifyColumnDbType(DbType.Int32, b => b.AsInt32());
        }

        [Test]
        public void CallingAsInt64SetsColumnDbTypeToInt64()
        {
            VerifyColumnDbType(DbType.Int64, b => b.AsInt64());
        }

        [Test]
        public void CallingAsStringSetsColumnDbTypeToString()
        {
            VerifyColumnDbType(DbType.String, b => b.AsString());
        }

        [Test]
        public void CallingAsStringWithSizeSetsColumnDbTypeToString()
        {
            VerifyColumnDbType(DbType.String, b => b.AsString(255));
        }

        [Test]
        public void CallingAsStringSetsColumnSizeToSpecifiedValue()
        {
            VerifyColumnSize(255, b => b.AsFixedLengthAnsiString(255));
        }

        [Test]
        public void CallingAsTimeSetsColumnDbTypeToTime()
        {
            VerifyColumnDbType(DbType.Time, b => b.AsTime());
        }

        [Test]
        public void CallingAsXmlSetsColumnDbTypeToXml()
        {
            VerifyColumnDbType(DbType.Xml, b => b.AsXml());
        }

        [Test]
        public void CallingAsXmlWithSizeSetsColumnDbTypeToXml()
        {
            VerifyColumnDbType(DbType.Xml, b => b.AsXml(255));
        }

        [Test]
        public void CallingAsXmlSetsColumnSizeToSpecifiedValue()
        {
            VerifyColumnSize(255, b => b.AsXml(255));
        }

        [Test]
        public void CallingAsCustomSetsTypeToNullAndSetsCustomType()
        {
            VerifyColumnProperty(c => c.Type = null, b => b.AsCustom("Test"));
            VerifyColumnProperty(c => c.CustomType = "Test", b => b.AsCustom("Test"));
        }

        [Test]
        public void CallingWithDefaultValueSetsDefaultValue()
        {
            const int value = 42;

            var expressions = new List<IMigrationExpression>();
            var contextMock = new Mock<IMigrationContext>();
            contextMock.Setup(x => x.Expressions).Returns(expressions);

            var columnMock = new Mock<ColumnDefinition>();

            var expressionMock = new Mock<AlterTableExpression>();

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object);
            builder.CurrentColumn = columnMock.Object;
            builder.WithDefaultValue(42);

            columnMock.VerifySet(c => c.DefaultValue = value);
        }

        [Test]
        public void CallingWithDefaultValueOnNewColumnDoesNotAddDefaultConstraintExpression() {
            const int value = 42;

            var expressions = new List<IMigrationExpression>();
            var contextMock = new Mock<IMigrationContext>();
            contextMock.Setup(x => x.Expressions).Returns(expressions);

            var columnMock = new Mock<ColumnDefinition>();
            columnMock.Setup(x => x.ModificationType).Returns(ColumnModificationType.Create);

            var expressionMock = new Mock<AlterTableExpression>();

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object);
            builder.CurrentColumn = columnMock.Object;
            builder.WithDefaultValue(42);

            Assert.That(expressions.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CallingWithDefaultValueOnAlterColumnAddsDefaultConstraintExpression() {
            const int value = 42;

            var expressions = new List<IMigrationExpression>();
            var contextMock = new Mock<IMigrationContext>();
            contextMock.Setup(x => x.Expressions).Returns(expressions);

            var columnMock = new Mock<ColumnDefinition>();
            columnMock.Setup(x => x.ModificationType).Returns(ColumnModificationType.Alter);

            var expressionMock = new Mock<AlterTableExpression>();

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object);
            builder.CurrentColumn = columnMock.Object;
            builder.WithDefaultValue(42);

            Assert.That(expressions.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CallingWithDefaultSetsDefaultValue()
        {
            var contextMock = new Mock<IMigrationContext>();

            var columnMock = new Mock<ColumnDefinition>();

            var expressionMock = new Mock<AlterTableExpression>();

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object);
            builder.CurrentColumn = columnMock.Object;
            builder.WithDefault(SystemMethods.CurrentDateTime);

            columnMock.VerifySet(c => c.DefaultValue = SystemMethods.CurrentDateTime);
        }

        [Test]
        public void CallingForeignKeySetsIsForeignKeyToTrue()
        {
            VerifyColumnProperty(c => c.IsForeignKey = true, b => b.ForeignKey());
        }

        [Test]
        public void CallingReferencedByAddsNewForeignKeyExpressionToContext()
        {
            var collectionMock = new Mock<ICollection<IMigrationExpression>>();

            var contextMock = new Mock<IMigrationContext>();
            contextMock.Setup(x => x.Expressions).Returns(collectionMock.Object);


            var columnMock = new Mock<ColumnDefinition>();
            columnMock.SetupGet(x => x.Name).Returns("BaconId");

            var expressionMock = new Mock<AlterTableExpression>();
            expressionMock.SetupGet(x => x.TableName).Returns("Bacon");

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object)
            {
                CurrentColumn = columnMock.Object
            };

            builder.ReferencedBy("fk_foo", "FooTable", "BarColumn");

            collectionMock.Verify(x => x.Add(It.Is<CreateForeignKeyExpression>(
                fk => fk.ForeignKey.Name == "fk_foo" &&
                        fk.ForeignKey.ForeignTable == "FooTable" &&
                        fk.ForeignKey.ForeignColumns.Contains("BarColumn") &&
                        fk.ForeignKey.ForeignColumns.Count == 1 &&
                        fk.ForeignKey.PrimaryTable == "Bacon" &&
                        fk.ForeignKey.PrimaryColumns.Contains("BaconId") &&
                        fk.ForeignKey.PrimaryColumns.Count == 1
                                                )));

            contextMock.VerifyGet(x => x.Expressions);
        }

        [Test]
        public void CallingForeignKeyAddsNewForeignKeyExpressionToContext()
        {
            var collectionMock = new Mock<ICollection<IMigrationExpression>>();

            var contextMock = new Mock<IMigrationContext>();
            contextMock.Setup(x => x.Expressions).Returns(collectionMock.Object);

            var columnMock = new Mock<ColumnDefinition>();
            columnMock.SetupGet(x => x.Name).Returns("BaconId");

            var expressionMock = new Mock<AlterTableExpression>();
            expressionMock.SetupGet(x => x.TableName).Returns("Bacon");

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object)
            {
                CurrentColumn = columnMock.Object
            };

            builder.ForeignKey("fk_foo", "FooTable", "BarColumn");

            collectionMock.Verify(x => x.Add(It.Is<CreateForeignKeyExpression>(
                fk => fk.ForeignKey.Name == "fk_foo" &&
                        fk.ForeignKey.PrimaryTable == "FooTable" &&
                        fk.ForeignKey.PrimaryColumns.Contains("BarColumn") &&
                        fk.ForeignKey.PrimaryColumns.Count == 1 &&
                        fk.ForeignKey.ForeignTable == "Bacon" &&
                        fk.ForeignKey.ForeignColumns.Contains("BaconId") &&
                        fk.ForeignKey.ForeignColumns.Count == 1
                                                )));

            contextMock.VerifyGet(x => x.Expressions);
        }

    [TestCase(Rule.Cascade), TestCase(Rule.SetDefault), TestCase(Rule.SetNull), TestCase(Rule.None)]
        public void CallingOnUpdateSetsOnUpdateOnForeignKeyExpression(Rule rule)
        {
            var builder = new AlterTableExpressionBuilder(null, null) { CurrentForeignKey = new ForeignKeyDefinition() };
            builder.OnUpdate(rule);
            Assert.That(builder.CurrentForeignKey.OnUpdate, Is.EqualTo(rule));
            Assert.That(builder.CurrentForeignKey.OnDelete, Is.EqualTo(Rule.None));
        }

        [TestCase(Rule.Cascade), TestCase(Rule.SetDefault), TestCase(Rule.SetNull), TestCase(Rule.None)]
        public void CallingOnDeleteSetsOnDeleteOnForeignKeyExpression(Rule rule)
        {
            var builder = new AlterTableExpressionBuilder(null, null) { CurrentForeignKey = new ForeignKeyDefinition() };
            builder.OnDelete(rule);
            Assert.That(builder.CurrentForeignKey.OnUpdate, Is.EqualTo(Rule.None));
            Assert.That(builder.CurrentForeignKey.OnDelete, Is.EqualTo(rule));
        }

        [TestCase(Rule.Cascade), TestCase(Rule.SetDefault), TestCase(Rule.SetNull), TestCase(Rule.None)]
        public void CallingOnDeleteOrUpdateSetsOnUpdateAndOnDeleteOnForeignKeyExpression(Rule rule)
        {
            var builder = new AlterTableExpressionBuilder(null, null) { CurrentForeignKey = new ForeignKeyDefinition() };
            builder.OnDeleteOrUpdate(rule);
            Assert.That(builder.CurrentForeignKey.OnUpdate, Is.EqualTo(rule));
            Assert.That(builder.CurrentForeignKey.OnDelete, Is.EqualTo(rule));
        }

        [Test]
        public void CallingIdentitySetsIsIdentityToTrue()
        {
            VerifyColumnProperty(c => c.IsIdentity = true, b => b.Identity());
        }

        [Test]
        public void CallingIndexedSetsIsIndexedToTrue()
        {
            VerifyColumnProperty(c => c.IsIndexed = true, b => b.Indexed());
        }

        [Test]
        public void CallingIndexedAddsIndexExpressionToContext()
        {
            var collectionMock = new Mock<ICollection<IMigrationExpression>>();

            var contextMock = new Mock<IMigrationContext>();
            contextMock.Setup(x => x.Expressions).Returns(collectionMock.Object);

            var columnMock = new Mock<ColumnDefinition>();
            columnMock.SetupGet(x => x.Name).Returns("BaconId");

            var expressionMock = new Mock<AlterTableExpression>();
            expressionMock.SetupGet(x => x.SchemaName).Returns("Eggs");
            expressionMock.SetupGet(x => x.TableName).Returns("Bacon");

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object) { CurrentColumn = columnMock.Object };

            builder.Indexed();

            collectionMock.Verify(x => x.Add(It.Is<CreateIndexExpression>(
                ix => ix.Index.Name == null
                      && ix.Index.TableName == "Bacon"
                      && ix.Index.SchemaName == "Eggs"
                      && !ix.Index.IsUnique
                      && !ix.Index.IsClustered
                      && ix.Index.Columns.All(c => c.Name == "BaconId")
                                                 )));

            contextMock.VerifyGet(x => x.Expressions);
        }

        [Test]
        public void CallingIndexedNamedAddsIndexExpressionToContext()
        {
            var collectionMock = new Mock<ICollection<IMigrationExpression>>();

            var contextMock = new Mock<IMigrationContext>();
            contextMock.Setup(x => x.Expressions).Returns(collectionMock.Object);

            var columnMock = new Mock<ColumnDefinition>();
            columnMock.SetupGet(x => x.Name).Returns("BaconId");

            var expressionMock = new Mock<AlterTableExpression>();
            expressionMock.SetupGet(x => x.SchemaName).Returns("Eggs");
            expressionMock.SetupGet(x => x.TableName).Returns("Bacon");

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object) { CurrentColumn = columnMock.Object };

            builder.Indexed("IX_Bacon_BaconId");

            collectionMock.Verify(x => x.Add(It.Is<CreateIndexExpression>(
                ix => ix.Index.Name == "IX_Bacon_BaconId"
                      && ix.Index.TableName == "Bacon"
                      && ix.Index.SchemaName == "Eggs"
                      && !ix.Index.IsUnique
                      && !ix.Index.IsClustered
                      && ix.Index.Columns.All(c => c.Name == "BaconId")
                                                 )));

            contextMock.VerifyGet(x => x.Expressions);
        }

        [Test]
        public void CallingPrimaryKeySetsIsPrimaryKeyToTrue()
        {
            VerifyColumnProperty(c => c.IsPrimaryKey = true, b => b.PrimaryKey());
        }

        [Test]
        public void CallingNullableSetsIsNullableToTrue()
        {
            VerifyColumnProperty(c => c.IsNullable = true, b => b.Nullable());
        }

        [Test]
        public void CallingNotNullableSetsIsNullableToFalse()
        {
            VerifyColumnProperty(c => c.IsNullable = false, b => b.NotNullable());
        }

        [Test]
        public void CallingUniqueSetsIsUniqueToTrue()
        {
            VerifyColumnProperty(c => c.IsUnique = true, b => b.Unique());
        }

        [Test]
        public void CallingUniqueAddsIndexExpressionToContext()
        {
            var collectionMock = new Mock<ICollection<IMigrationExpression>>();

            var contextMock = new Mock<IMigrationContext>();
            contextMock.Setup(x => x.Expressions).Returns(collectionMock.Object);

            var columnMock = new Mock<ColumnDefinition>();
            columnMock.SetupGet(x => x.Name).Returns("BaconId");

            var expressionMock = new Mock<AlterTableExpression>();
            expressionMock.SetupGet(x => x.SchemaName).Returns("Eggs");
            expressionMock.SetupGet(x => x.TableName).Returns("Bacon");

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object) { CurrentColumn = columnMock.Object };

            builder.Unique();

            collectionMock.Verify(x => x.Add(It.Is<CreateIndexExpression>(
                ix => ix.Index.Name == null
                      && ix.Index.TableName == "Bacon"
                      && ix.Index.SchemaName == "Eggs"
                      && ix.Index.IsUnique
                      && !ix.Index.IsClustered
                      && ix.Index.Columns.All(c => c.Name == "BaconId")
                                                 )));

            contextMock.VerifyGet(x => x.Expressions);
        }

        [Test]
        public void CallingUniqueNamedAddsIndexExpressionToContext()
        {
            var collectionMock = new Mock<ICollection<IMigrationExpression>>();

            var contextMock = new Mock<IMigrationContext>();
            contextMock.Setup(x => x.Expressions).Returns(collectionMock.Object);

            var columnMock = new Mock<ColumnDefinition>();
            columnMock.SetupGet(x => x.Name).Returns("BaconId");

            var expressionMock = new Mock<AlterTableExpression>();
            expressionMock.SetupGet(x => x.SchemaName).Returns("Eggs");
            expressionMock.SetupGet(x => x.TableName).Returns("Bacon");

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object) { CurrentColumn = columnMock.Object };

            builder.Unique("IX_Bacon_BaconId");

            collectionMock.Verify(x => x.Add(It.Is<CreateIndexExpression>(
                ix => ix.Index.Name == "IX_Bacon_BaconId"
                      && ix.Index.TableName == "Bacon"
                      && ix.Index.SchemaName == "Eggs"
                      && ix.Index.IsUnique
                      && !ix.Index.IsClustered
                      && ix.Index.Columns.All(c => c.Name == "BaconId")
                                                 )));

            contextMock.VerifyGet(x => x.Expressions);
        }

        private void VerifyColumnProperty(Action<ColumnDefinition> columnExpression, Action<AlterTableExpressionBuilder> callToTest)
        {
            var columnMock = new Mock<ColumnDefinition>();

            var expressionMock = new Mock<AlterTableExpression>();

            var contextMock = new Mock<IMigrationContext>();
            contextMock.SetupGet(mc => mc.Expressions).Returns(new Collection<IMigrationExpression>());

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object);
            builder.CurrentColumn = columnMock.Object;

            callToTest(builder);

            columnMock.VerifySet(columnExpression);

        }

        private void VerifyColumnDbType(DbType expected, Action<AlterTableExpressionBuilder> callToTest)
        {
            var columnMock = new Mock<ColumnDefinition>();

            var expressionMock = new Mock<AlterTableExpression>();

            var contextMock = new Mock<IMigrationContext>();

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object);
            builder.CurrentColumn = columnMock.Object;

            callToTest(builder);

            columnMock.VerifySet(c => c.Type = expected);
        }

        private void VerifyColumnSize(int expected, Action<AlterTableExpressionBuilder> callToTest)
        {
            var columnMock = new Mock<ColumnDefinition>();

            var expressionMock = new Mock<AlterTableExpression>();

            var contextMock = new Mock<IMigrationContext>();

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object);
            builder.CurrentColumn = columnMock.Object;

            callToTest(builder);

            columnMock.VerifySet(c => c.Size = expected);
        }

        private void VerifyColumnPrecision(int expected, Action<AlterTableExpressionBuilder> callToTest)
        {
            var columnMock = new Mock<ColumnDefinition>();

            var expressionMock = new Mock<AlterTableExpression>();

            var contextMock = new Mock<IMigrationContext>();

            var builder = new AlterTableExpressionBuilder(expressionMock.Object, contextMock.Object);
            builder.CurrentColumn = columnMock.Object;

            callToTest(builder);

            columnMock.VerifySet(c => c.Precision = expected);
        }
    }
}
