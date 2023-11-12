using SecretSanta.Infra.Files.API.Interfaces;
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
        public void Should_read_lines_from_file()
        {
            // GIVEN
            this.mockFileSystem.File.WriteAllLines("filename", new string[] 
            {
                "line1",
                "line2"
            });

            // WHEN
            var readLines = this.fileService_sut.ReadLinesFromFile("filename");

            // THEN
            Assert.That(readLines.Count, Is.EqualTo(2));
            Assert.That(readLines[0], Is.EqualTo("line1"));
            Assert.That(readLines[1], Is.EqualTo("line2"));
        }

        [Test]
        public void Should_throw_exception_if_file_does_not_exist_upon_reading()
        {
            Assert.Throws<FileNotFoundException>(() => this.fileService_sut.ReadLinesFromFile("filename"));
        }
    }
}
