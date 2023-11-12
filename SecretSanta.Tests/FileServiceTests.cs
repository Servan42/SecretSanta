using SecretSanta.Infra.Files.API.Interfaces;
using SecretSanta.Infra.Files.API.Model;
using SecretSanta.Infra.Files.API.Services;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Tests
{
    internal class FileServiceTests
    {
        IFileSystem mockFileSystem;
        IFileService fileService_sut;

        [SetUp]
        public void Setup()
        {
            this.mockFileSystem = new MockFileSystem();
            fileService_sut = new FileService(mockFileSystem);
        }

        [Test]
        public void Should_write_lines_to_file()
        {
            // GIVEN
            var lines = new List<string> 
            {
                "line1",
                "line2"
            };

            // WHEN
            this.fileService_sut.WriteLinesToFile("filename", lines);

            // THEN
            var wroteLines = this.mockFileSystem.File.ReadAllLines("filename");
            Assert.That(wroteLines.Count, Is.EqualTo(2));
            Assert.That(wroteLines[0], Is.EqualTo("line1"));
            Assert.That(wroteLines[1], Is.EqualTo("line2"));
        }

        [Test]
        public void Should_read_members_from_file()
        {
            // GIVEN
            this.mockFileSystem.File.WriteAllLines("memberList", new string[] 
            {
                " A",
                "",
                "B "
            });

            // WHEN
            var members = this.fileService_sut.ReadMembersFromFile("memberList");

            // THEN
            Assert.That(members.Count, Is.EqualTo(2));
            Assert.That(members[0], Is.EqualTo("A"));
            Assert.That(members[1], Is.EqualTo("B"));
        }

        [Test]
        public void Should_read_members_with_email_from_file()
        {
            // GIVEN
            this.mockFileSystem.File.WriteAllLines("membersWithEmail.csv", new string[]
            {
                "member,email",
                "Alice,alice.liddell@mail.com",
                "Bob,bob@mail.com"
            });

            // WHEN
            var result = this.fileService_sut.ReadMembersWithEmailFromFile("membersWithEmail.csv");

            // THEN
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Member, Is.EqualTo("Alice"));
            Assert.That(result[0].Email.Address, Is.EqualTo("alice.liddell@mail.com"));
            Assert.That(result[1].Member, Is.EqualTo("Bob"));
            Assert.That(result[1].Email.Address, Is.EqualTo("bob@mail.com"));
        }

        [Test]
        public void Should_throw_exception_if_not_a_members_with_email_file()
        {
            // GIVEN
            this.mockFileSystem.File.WriteAllLines("membersWithEmail.csv", new string[]
            {
                "Alice,alice.liddell@mail.com",
                "Bob,bob@mail.com"
            });

            // WHEN
            var ex = Assert.Throws<FileServiceException>(() => this.fileService_sut.ReadMembersWithEmailFromFile("membersWithEmail.csv"));

            Assert.That(ex.Message, Is.EqualTo("membersWithEmail.csv was not recognized as a membersWithEmail file. It must contain CSV headers member,email"));
        }

        [Test]
        public void Should_throw_exception_if_members_with_email_file_contains_a_wrong_line()
        {
            // GIVEN
            this.mockFileSystem.File.WriteAllLines("membersWithEmail.csv", new string[]
            {
                "member,email",
                "Alice",
                "Bob,bob@mail.com"
            });

            // WHEN
            var ex = Assert.Throws<FileServiceException>(() => this.fileService_sut.ReadMembersWithEmailFromFile("membersWithEmail.csv"));

            Assert.That(ex.Message, Is.EqualTo("membersWithEmail.csv contains an unrecognized line: Alice"));
        }

        [Test]
        public void Should_read_constraints_from_file()
        {
            // GIVEN
            this.mockFileSystem.File.WriteAllLines("constraints.csv", new string[]
            {
                "CannotGiftToMemberB,CannotReceiveFromMemberA,IsViceVersa",
                "A ,B,True",
                "",
                "C, D,false"
            });

            // WHEN
            var constraints = this.fileService_sut.ReadConstraintsFromFile("constraints.csv");

            // THEN
            Assert.That(constraints.Count, Is.EqualTo(2));
            Assert.That(constraints[0].CannotGiftToMemberB, Is.EqualTo("A"));
            Assert.That(constraints[0].CannotReceiveFromMemberA, Is.EqualTo("B"));
            Assert.That(constraints[0].IsViceVersa, Is.EqualTo(true));
            Assert.That(constraints[1].CannotGiftToMemberB, Is.EqualTo("C"));
            Assert.That(constraints[1].CannotReceiveFromMemberA, Is.EqualTo("D"));
            Assert.That(constraints[1].IsViceVersa, Is.EqualTo(false));
        }

        [Test]
        public void Should_throw_exception_if_not_a_constraits_file()
        {
            // GIVEN
            this.mockFileSystem.File.WriteAllLines("constraints.csv", new string[]
            {
                "A,B,True",
                "C,D,false"
            });

            // WHEN
            var ex = Assert.Throws<FileServiceException>(() => this.fileService_sut.ReadConstraintsFromFile("constraints.csv"));

            Assert.That(ex.Message, Is.EqualTo("constraints.csv was not recognized as a constraint file. It must contain CSV headers CannotGiftToMemberB,CannotReceiveFromMemberA,IsViceVersa"));
        }

        [Test]
        public void Should_throw_exception_if_constraits_file_contains_a_wrong_line()
        {
            // GIVEN
            this.mockFileSystem.File.WriteAllLines("constraints.csv", new string[]
            {
                "CannotGiftToMemberB,CannotReceiveFromMemberA,IsViceVersa",
                "A,B",
                "C,D,false"
            });

            // WHEN
            var ex = Assert.Throws<FileServiceException>(() => this.fileService_sut.ReadConstraintsFromFile("constraints.csv"));

            Assert.That(ex.Message, Is.EqualTo("constraints.csv contains an unrecognized line: A,B"));
        }

        [Test]
        public void Should_throw_exception_if_file_does_not_exist_upon_reading_members()
        {
            Assert.Throws<FileNotFoundException>(() => this.fileService_sut.ReadMembersFromFile("filename"));
        }

        [Test]
        public void Should_throw_exception_if_file_does_not_exist_upon_reading_constraints()
        {
            Assert.Throws<FileNotFoundException>(() => this.fileService_sut.ReadConstraintsFromFile("filename"));
        }
    }
}
