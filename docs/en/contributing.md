# Contributing Guide

Thank you for your interest in contributing to SiliconLifeCollective!

## Code of Conduct

This project follows the Apache 2.0 License. Be respectful and professional in all interactions.

---

## Getting Started

### 1. Fork the Repository

Click the "Fork" button on GitHub to create your own copy.

### 2. Clone Your Fork

```bash
git clone https://github.com/your-username/SiliconLifeCollective.git
cd SiliconLifeCollective
```

### 3. Set Up Development Environment

```bash
# Install .NET 9 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run tests
dotnet test
```

### 4. Create a Feature Branch

```bash
git checkout -b feature/your-feature-name
```

---

## Development Workflow

### Code Style

- Follow C# coding conventions
- Use PascalCase for class names
- Use camelCase for method parameters
- Use `_camelCase` for private fields
- All public APIs must have XML documentation

### Commit Messages

Follow **Conventional Commits** format:

```
<type>(<scope>): <description>
```

**Types**:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code formatting
- `refactor`: Code refactoring
- `test`: Test changes
- `chore`: Build/tooling changes

**Examples**:
```bash
feat(localization): add Korean language support
fix(permission): fix null pointer in callback
docs: update contributing guide
refactor(web): simplify controller structure
```

### Making Changes

1. **Write Code**
   - Follow existing patterns
   - Add tests for new features
   - Update documentation

2. **Test Your Changes**
   ```bash
   # Run all tests
   dotnet test
   
   # Build in release mode
   dotnet build --configuration Release
   ```

3. **Format Code**
   ```bash
   dotnet format
   ```

4. **Commit Changes**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```

5. **Push to Your Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Create Pull Request**
   - Go to the original repository
   - Click "Compare & pull request"
   - Fill in the PR template
   - Submit

---

## Pull Request Guidelines

### PR Title

Use the same format as commit messages:
```
feat(localization): add Korean language support
```

### PR Description

Include:

1. **What** - What does this PR do?
2. **Why** - Why is this change needed?
3. **How** - How did you implement it?
4. **Testing** - How was it tested?

### Example PR Description

```markdown
## What
Add Korean language localization for all UI components and documentation.

## Why
Expand the project's accessibility to Korean-speaking users.

## How
- Created KoKR.cs localization file
- Added 500+ translation keys
- Updated all views to use localization
- Created Korean documentation in docs/ko-KR/

## Testing
- Verified all UI elements display Korean correctly
- Tested language switching functionality
- Reviewed translations with native speaker
```

---

## Contribution Types

### 1. Bug Fixes

**Process**:
1. Check existing issues
2. Create issue if none exists
3. Fix the bug
4. Add test case
5. Submit PR

**Requirements**:
- Clear description of the bug
- Steps to reproduce
- Test that prevents regression

### 2. New Features

**Process**:
1. Discuss feature in Issues/Discussions
2. Get approval from maintainers
3. Implement feature
4. Add comprehensive tests
5. Update documentation
6. Submit PR

**Requirements**:
- Feature proposal approved
- Full test coverage
- Documentation updated
- Backward compatible

### 3. Documentation

**Process**:
1. Identify documentation gap
2. Write/update documentation
3. Submit PR

**Requirements**:
- Clear and concise
- Examples included
- Multi-language if applicable

### 4. Code Refactoring

**Process**:
1. Propose refactoring in Issue
2. Get approval
3. Refactor code
4. Ensure all tests pass
5. Submit PR

**Requirements**:
- No functional changes
- All tests pass
- Improved code quality
- Clear explanation

---

## Testing Guidelines

### Unit Tests

```csharp
[TestMethod]
public void MyFeature_ShouldWork_AsExpected()
{
    // Arrange
    var service = new MyService();
    
    // Act
    var result = service.DoSomething();
    
    // Assert
    Assert.IsTrue(result.Success);
}
```

### Integration Tests

Test complete workflows:
- AI interaction
- Tool execution
- Permission validation
- Storage operations

### Manual Testing

For UI changes:
- Test in multiple browsers
- Verify responsive design
- Check accessibility

---

## Documentation Guidelines

### Code Comments

- XML comments for all public APIs
- Inline comments for complex logic
- Use English for code comments

### Documentation Files

- Place in `docs/{language}/`
- Update all language versions
- Follow existing structure

### Multi-Language Docs

When adding documentation:
1. Create English version first
2. Translate to other languages
3. Keep content synchronized

---

## Review Process

### What Maintainers Check

1. **Code Quality**
   - Follows conventions
   - Clean and readable
   - Well-documented

2. **Tests**
   - Adequate coverage
   - All tests pass
   - Edge cases covered

3. **Documentation**
   - Updated
   - Clear explanations
   - Multi-language

4. **Compatibility**
   - Backward compatible
   - No breaking changes (without notice)
   - Follows semantic versioning

### Review Timeline

- Initial review: 1-3 days
- Feedback incorporation: As needed
- Merge: After approval

---

## Common Issues

### PR Rejected

**Reasons**:
- Doesn't follow guidelines
- Insufficient testing
- Breaking changes without notice
- Poor code quality

**Solution**:
- Address feedback
- Update PR
- Resubmit

### Merge Conflicts

**Solution**:
```bash
# Update your branch
git fetch origin
git rebase origin/master

# Resolve conflicts
# Edit conflicted files
git add .
git rebase --continue

# Force push
git push --force-with-lease
```

---

## Getting Help

### Resources

- **Documentation**: [docs/](../)
- **Issues**: GitHub Issues
- **Discussions**: GitHub Discussions
- **Code of Conduct**: CODE_OF_CONDUCT.md

### Contact

- Create an Issue for bugs
- Start a Discussion for questions
- Tag maintainers for urgent matters

---

## Recognition

Contributors will be recognized in:
- README.md contributors section
- Release notes
- Project documentation

---

## License

By contributing, you agree that your contributions will be licensed under the Apache 2.0 License.

---

## Next Steps

- 📚 Read the [Documentation](../)
- 🐛 Check [Open Issues](https://github.com/your-org/SiliconLifeCollective/issues)
- 💬 Start a [Discussion](https://github.com/your-org/SiliconLifeCollective/discussions)
- 🚀 Fork and start contributing!

Thank you for contributing to SiliconLifeCollective! 🎉
